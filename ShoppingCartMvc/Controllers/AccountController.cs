using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartMvc.Models;
using ShoppingCartMvc.Models.CustomIdentity;

namespace ShoppingCartMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<CustomIdentityUser> _signInManager;
        private readonly UserManager<CustomIdentityUser> _userManager;

        public AccountController(SignInManager<CustomIdentityUser> signInManager, UserManager<CustomIdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Login(string ReturnUrl)
        {
            //CustomIdentityUser user = new CustomIdentityUser
            //{
            //    UserName = "mert",
            //    Email = "mert@gmail.com",
            //    IsActive = true,
            //    NameSurname = "Mert Kaya"
            //};
            //var result = _userManager.CreateAsync(user, "123456").Result;
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                var userControl = await _userManager.FindByNameAsync(model.Username);
                if (userControl == null)
                    throw new Exception("User not found");
                if (!userControl.IsActive)
                    throw new Exception("User inactive.");
                var loginResult = await _signInManager.PasswordSignInAsync(userControl, model.Password, model.RememberMe, true);
                if (loginResult.Succeeded)
                    return Json(new GeneralResponse<string>
                    {
                        Result = model.ReturnUrl
                    });
                else
                    throw new Exception("Username or password is wrong");
            }
            catch (Exception ex)
            {
                return Json(new GeneralResponse<string>
                {
                    IsError = true,
                    ErrorMessage = ex.Message
                });
            }
        }

        public async Task<IActionResult> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
