using ProjectLab.Models.Views;
using ProjectLab.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectLab.Controllers
{
    public class AccountsController : Controller
    {

        private readonly IUserService _userService;

        public AccountsController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        public IActionResult Menu()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (_userService.IsUserLoggedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }

            return View("~/Views/Accounts/Register.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModelView model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _userService.Register(model);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Login");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                return View(model);
            }
            catch (Exception e)
            {
                return View("~/Views/Accounts/Register.cshtml");
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (_userService.IsUserLoggedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }

            return View("~/Views/Accounts/Login.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModelView model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _userService.Login(model);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError("", "Invalid login attempt");
                }

                return RedirectToAction("Login");
            }
            catch
            {

                return View("~/Views/Accounts/Login.cshtml");
            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _userService.Logout();
            return RedirectToAction("Index", "Home");
        }

    }
}
