using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyOnlineShop.Common.Constants;
using MyOnlineShop.Common.Services;
using MyOnlineShop.WebMVC.Data.Models.Customers;
using MyOnlineShop.WebMVC.Services.Identity;
using MyOnlineShop.WebMVC.ViewModels.Identity;
using System;
using System.Threading.Tasks;

namespace MyOnlineShop.WebMVC.Controllers
{
    [AllowAnonymous]
    public class IdentityController : HandleController
    {
        private readonly IIdentityService identityService;
        private readonly ICurrentUserService currentUserService;
        private readonly IMapper mapper;

        public IdentityController(
            IIdentityService identityService,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            this.identityService = identityService;
            this.currentUserService = currentUserService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Login(string returnUrl)
        {
            returnUrl ??= Url.Content("~/");

            if (!string.IsNullOrWhiteSpace(currentUserService.UserId))
            {
                return this.Redirect(returnUrl);
            }

            return await Task.Run(() => View(new LoginViewModel()));
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            loginViewModel.ReturnUrl ??= Url.Content("~/");

            if (!this.ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");

                return this.View(loginViewModel);
            }

            return await this.Handle(
                async () =>
                {
                    var token = await this.identityService
                                          .Login(this.mapper.Map<LoginViewModel, LoginCustomerInputModel>(loginViewModel));

                    this.Response
                        .Cookies
                        .Append(
                                AuthConstants.AuthenticationCookieName,
                                token,
                                new CookieOptions
                                {
                                    HttpOnly = true,
                                    Secure = true,
                                    MaxAge = TimeSpan.FromDays(1)
                                });

                },
                success: this.LocalRedirect("~/"),
                failure: this.View(loginViewModel));
        }

        public async Task<IActionResult> Register()
        {
            return await Task.Run(() => this.View(new RegisterViewModel()));
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            registerViewModel.ReturnUrl ??= Url.Content("~/");

            return await this.Handle(
                async () =>
                {
                    var token = await this.identityService
                         .Register(this.mapper.Map<RegisterViewModel, RegisterCustomerInputModel>(registerViewModel));

                    this.Response
                        .Cookies
                        .Append(
                           AuthConstants.AuthenticationCookieName,
                           token,
                           new CookieOptions
                           {
                               HttpOnly = true,
                               Secure = true,
                               MaxAge = TimeSpan.FromDays(1)
                           });
                },
                success: this.LocalRedirect(registerViewModel.ReturnUrl),
                failure: this.View(registerViewModel));
        }

        [HttpPost]
        public IActionResult Logout(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            this.Response
                .Cookies
                .Delete("Authentication");

            return this.LocalRedirect(returnUrl);
        }
    }
}
