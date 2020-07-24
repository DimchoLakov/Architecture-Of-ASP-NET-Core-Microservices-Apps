using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyOnlineShop.Common.Constants;
using MyOnlineShop.Common.Services;
using MyOnlineShop.WebMVC.Admin.Services.Identity;
using MyOnlineShop.WebMVC.Admin.ViewModels.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyOnlineShop.WebMVC.Admin.Controllers
{
    [AllowAnonymous]
    public class IdentityController : Controller
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

            try
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
                                    Secure = false,
                                    MaxAge = TimeSpan.FromDays(1)
                                });

                return this.LocalRedirect(loginViewModel.ReturnUrl);
            }
            catch (Refit.ApiException apiEx)
            {
                if (apiEx.HasContent)
                {
                    JsonConvert
                        .DeserializeObject<List<string>>(apiEx.Content)
                        .ForEach(error => this.ModelState.AddModelError(string.Empty, error));
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, ErrorConstants.InternalServerErrorMessage);
                }

                this.HandleException(apiEx);
            }

            return this.View(loginViewModel);
        }

        public async Task<IActionResult> Register()
        {
            return await Task.Run(() => this.View(new RegisterViewModel()));
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            registerViewModel.ReturnUrl ??= Url.Content("~/");

            try
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
                               Secure = false,
                               MaxAge = TimeSpan.FromDays(1)
                           });

                return this.LocalRedirect(registerViewModel.ReturnUrl);
            }
            catch (Refit.ApiException apiEx)
            {
                if (apiEx.HasContent)
                {
                    JsonConvert
                        .DeserializeObject<List<string>>(apiEx.Content)
                        .ForEach(error => this.ModelState.AddModelError(string.Empty, error));
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, ErrorConstants.InternalServerErrorMessage);
                }

                this.HandleException(apiEx);
            }

            return this.View(registerViewModel);
        }

        [HttpPost]
        public IActionResult Logout(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            this.Response
                .Cookies
                .Delete(AuthConstants.AuthenticationCookieName);

            return this.LocalRedirect(returnUrl);
        }

        private void HandleException(Exception ex)
        {
            ViewBag.IdentityServiceInoperativeMsg = $"Identity Service is inoperative {ex.GetType().Name} - {ex.Message}";
        }
    }
}
