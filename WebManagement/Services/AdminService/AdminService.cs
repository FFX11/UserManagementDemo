using DataLibrary.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using Blazored.LocalStorage;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;

namespace WebManagement.Services.AuthenticationService
{
    public class AdminService : IAdminService
    {
        private readonly HttpClient _httpClient;

        public AdminService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task DeleteUser(string id)
        {
            var result = await _httpClient.DeleteAsync($"https://localhost:7023/api/administration/DeleteUser/{id}");
            //var errors = await result.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>();
        }

        public async Task<EditUserModel> EditSaveUser(EditUserModel model)
        {

            var result = await _httpClient.PostAsJsonAsync($"https://localhost:7023/api/administration/EditSaveUser/", model);
            //var errors = await result.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>();

            if (result.IsSuccessStatusCode == false)
            {
                //CustomValidator custom = new CustomValidator();
                //custom.DisplayErrors(errors);

            }
            return model;
        }

        public async Task<EditUserModel> EditUser(string id)
        {
            var result = await _httpClient.GetFromJsonAsync<EditUserModel>($"https://localhost:7023/api/administration/edituser/{id}");

            if (result != null)
            {
                return result;
            }
            else
            {
                throw new Exception("not found!");
            }
        }

        public async Task EditUsersToRole(List<UserRoleModel> models, string id)
        {
            var result = await _httpClient.PostAsJsonAsync($"https://localhost:7023/api/administration/editusersinrole/{id}", models);

            if (result.IsSuccessStatusCode == false)
            {
                throw new Exception(result.ReasonPhrase);
            }

        }

        public async Task<List<UserRoleModel>> EditUserInRole(string id)
        {
            var result = await _httpClient.GetFromJsonAsync<List<UserRoleModel>>($"https://localhost:7023/api/administration/geteditbyrole/{id}");

            if (result != null)
            {
                return result;
            }
            else
            {
                throw new Exception("not found!");
            }
        }

        public async Task DeleteRoleById(string id)
        {
            var result = await _httpClient.DeleteAsync($"https://localhost:7023/api/Administration/DeleteRoleById/{id}");
            //var errors = await result.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>();

            //if (result.IsSuccessStatusCode == false)
            //{
            //    CustomValidation custom = new CustomValidation();
            //    custom.DisplayErrors(errors);
            //}

        }

        public async Task EditRole(EditRoleModel model)
        {
            var result = await _httpClient.PostAsJsonAsync($"https://localhost:7023/api/Administration/editrole/", model);
            var errors = await result.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>();

            //if (result.IsSuccessStatusCode == false)
            //{
            //    CustomValidation custom = new CustomValidation();
            //    custom.DisplayErrors(errors);
            //}

        }

        public async Task<EditRoleModel> GetRoleById(string id)
        {
            var result = await _httpClient.GetFromJsonAsync<EditRoleModel>($"https://localhost:7023/api/Administration/getrole/{id}");
            if (result != null)
            {
                return result;
            }
            throw new Exception("not found!");
        }

        public async Task GetRoles()
        {
            var result = await _httpClient.GetAsync("https://localhost:7023/api/Administration/roles");

            if (result.IsSuccessStatusCode == false)
            {
                throw new Exception(result.ReasonPhrase);
            }
        }

        public async Task GetUsers()
        {
            var result = await _httpClient.GetAsync("https://localhost:7023/api/accounts/getusers");

            if (result.IsSuccessStatusCode == false)
            {
                throw new Exception(result.ReasonPhrase);
            }
        }

        public async Task CreateRole(CreateRoleModel model)
        {
            var result = await _httpClient.PostAsJsonAsync("https://localhost:7023/api/Administration/createrole", model);

            if (result.IsSuccessStatusCode == false)
            {
                throw new Exception(result.ReasonPhrase);
            }
        }
    }
}
