using DataLibrary.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WebManagement.Pages;

namespace WebManagement.Services.AuthenticationService
{
    public interface IAuthService
    {
        Task<LoginResult> Login(LoginModel loginModel);
        Task Logout();
        Task Register(RegisterModel registerModel);
    }
}