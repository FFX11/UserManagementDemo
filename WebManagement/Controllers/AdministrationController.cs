using DataLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;

namespace WebManagement.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministrationController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserRoleModel> _logger;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ILogger<UserRoleModel> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return BadRequest($"User with Id = {id} cannot be found");
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);

                }
            }
            return BadRequest(ModelState);
        }


        [HttpPost("EditSaveUser")]
        public async Task<IActionResult> EditSaveUser(EditUserModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                return BadRequest($"User with Id = {model.Id} cannot be found");
            }
            else
            {
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.City = model.City;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return Ok(result);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                        
                    }
                }
                return BadRequest(ModelState);
            }

        }

        [HttpGet("EditUser/{id}")]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return BadRequest($"User with Id = {id} cannot be found");
            }

            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new EditUserModel
            {
                Id = id,
                Email = user.Email,
                UserName = user.UserName,
                City = user.City,
                Claims = userClaims.Select(x => x.Value).ToList(),
                Roles = userRoles
            };

            return Ok(model);
        }

        [HttpPost("EditUsersInRole/{id}")]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleModel> models, string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return BadRequest("Role not found");
            }

            for (int i = 0; i < models.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(models[i].UserId);
                IdentityResult result = null;

                if (models[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!models[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (models.Count - 1))
                    {
                        continue;
                    }
                    else
                    {
                        return Ok(result);
                    }
                }
            }

            return Ok();
        }

        [HttpGet("GetEditByRole/{id}")]
        public async Task<IActionResult> GetUserByRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return BadRequest($"Role with Id = {id} cannot be found");
            }
            var model = new List<UserRoleModel>();

            foreach (var user in _userManager.Users)
            {
                var userRoleModel = new UserRoleModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleModel.IsSelected = true;
                }
                else
                {
                    userRoleModel.IsSelected = false;
                }
                model.Add(userRoleModel);
                
            }

            return Ok(model);
        }

        [HttpDelete("DeleteRoleById/{id}")]
        public async Task<IActionResult> DeleteRoleById(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return BadRequest($"Role with Id = {id} cannot be found");
            }

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);

                }
            }
            return BadRequest(ModelState);
        }

        [HttpGet("GetRole/{id}")]
        public async Task<IActionResult> GetRoleById(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return BadRequest($"Role with Id = {id} cannot be found");
            }

            var model = new EditRoleModel
            {
                Id = role.Id,
                RoleName = role.Name,
            };

            IList<ApplicationUser> users = await _userManager.GetUsersInRoleAsync(role.Name);
            foreach (var user in users)
                model.Users.Add(user.UserName);

            return Ok(model);
        }

        [HttpPost("EditRole")]
        public async Task<IActionResult> EditRole(EditRoleModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                return BadRequest($"Role with Id = { model.Id } cannot be found");
            }
            else
            {
                role.Name = model.RoleName;
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return Ok(result);
                }
           
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return BadRequest(ModelState);
            }
        }

        [HttpGet("roles")]
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles;

            return Ok(roles);
        }

        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(CreateRoleModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole { Name = model.RoleName };

                IdentityResult result = await _roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return Ok(result);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return Ok(model);
            
        }

        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            var users = _userManager.Users;

            if (users == null)
                return BadRequest("Bad request");

            return Ok(users);
        }
    }
}
