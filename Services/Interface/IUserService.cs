using ProjectLab.Models.Entities;
using ProjectLab.Models.Views;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ProjectLab.Services.Interface
{
    public interface IUserService
    {
        public User GetUser(string userId);

        public bool IsUserLoggedIn(ClaimsPrincipal User);

        public Task<IdentityResult> Register(RegisterModelView model);

        public Task<SignInResult> Login(LoginModelView model);

        public Task Logout();
    }
}
