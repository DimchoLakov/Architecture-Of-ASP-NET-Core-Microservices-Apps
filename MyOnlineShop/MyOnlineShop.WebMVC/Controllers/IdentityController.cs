using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyOnlineShop.WebMVC.Data.Models.Customers;
using MyOnlineShop.WebMVC.ViewModels.Identity;
using System.Threading.Tasks;

namespace MyOnlineShop.WebMVC.Controllers
{
    public class IdentityController : Controller
    {
        private readonly SignInManager<Customer> signInManager;
        private readonly UserManager<Customer> userManager;


        public IdentityController(
            SignInManager<Customer> signInManager,
            UserManager<Customer> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Login(string returnUrl)
        {
            returnUrl ??= Url.Content("~/");

            if (this.User.Identity.IsAuthenticated)
            {
                return this.Redirect(returnUrl);
            }

            await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            var loginViewModel = new LoginViewModel();

            return View(loginViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            loginViewModel.ReturnUrl ??= Url.Content("~/");

            if (this.ModelState.IsValid)
            {
                var result = await this.signInManager
                    .PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return LocalRedirect(loginViewModel.ReturnUrl);
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");

            return View(loginViewModel);
        }

        public IActionResult Register()
        {
            var registerViewModel = new RegisterViewModel();

            return this.View(registerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            registerViewModel.ReturnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = new Customer
                {
                    UserName = registerViewModel.Email,
                    Email = registerViewModel.Email,
                    FirstName = registerViewModel.FirstName,
                    LastName = registerViewModel.LastName
                };
                var result = await this.userManager.CreateAsync(user, registerViewModel.Password);
                if (result.Succeeded)
                {
                    await this.signInManager.SignInAsync(user, isPersistent: false);
                    return this.LocalRedirect(registerViewModel.ReturnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return this.View(registerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            await this.signInManager.SignOutAsync();

            return this.LocalRedirect(returnUrl);
        }
    }
}
