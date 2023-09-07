using ProjectLab.Data.Repositories.Interface;
using ProjectLab.Models.Entities;
using ProjectLab.Models.Views;
using ProjectLab.Services.Interface;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ProjectLab.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, IUserRepository userRepository)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userRepository = userRepository;
        }

        public User GetUser(string userId)
        {
            return _userRepository.FindByCondition(user => user.Id == userId).FirstOrDefault();
        }

        public bool IsUserLoggedIn(ClaimsPrincipal User)
        {
            return _signInManager.IsSignedIn(User);
        }

        public async Task<IdentityResult> Register(RegisterModelView model)
        {

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber
            };

            using (var stream = new BinaryReader(model.Photo.OpenReadStream()))
            {
                user.Photo = stream.ReadBytes((int)model.Photo.Length);
            }

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Client");
            }

            return result;
        }

        public async Task<SignInResult> Login(LoginModelView model)
        {
            SignInResult signInResult = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            return signInResult;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

    }
}
