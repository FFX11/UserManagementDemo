using DataLibrary.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WebManagement.Pages;

namespace WebManagement.Services.AuthenticationService
{
    public interface IAdminService
    {
        Task DeleteRoleById(string id);
        Task DeleteUser(string id);
        Task<EditUserModel> EditSaveUser(EditUserModel model);
        Task<EditUserModel> EditUser(string id);
        Task EditUsersToRole(List<UserRoleModel> models, string id);
        Task<List<UserRoleModel>> EditUserInRole(string id);
        Task EditRole(EditRoleModel model);
        Task<EditRoleModel> GetRoleById(string id);
        Task GetRoles();
        Task CreateRole(CreateRoleModel model);
        Task GetUsers();
    }
}