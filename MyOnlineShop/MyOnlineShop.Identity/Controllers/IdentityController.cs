using Microsoft.AspNetCore.Mvc;
using MyOnlineShop.Common.Controllers;
using MyOnlineShop.Identity.Models;
using MyOnlineShop.Identity.Services.Identity;
using System.Threading.Tasks;

namespace MyOnlineShop.Identity.Controllers
{
    public class IdentityController : ApiController
    {
        private readonly IIdentityService identityService;

        public IdentityController(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        [HttpPost(nameof(Login))]
        public async Task<ActionResult<string>> Login(LoginCustomerInputModel loginCustomerInputModel)
        {
            var result = await this.identityService.LoginAsync(loginCustomerInputModel);

            if (!result.Succeeded)
            {
                return this.BadRequest(result.Errors);
            }

            return result.Token;
        }

        [HttpPost(nameof(Register))]
        public async Task<ActionResult<string>> Register(RegisterCustomerInputModel registerCustomerInputModel)
        {
            var result = await this.identityService.RegisterAsync(registerCustomerInputModel);

            if (!result.Succeeded)
            {
                return this.BadRequest(result.Errors);
            }

            var loginCustomerInputModel = new LoginCustomerInputModel
            {
                Email = result.Email,
                Password = result.Password
            };

            return await this.Login(loginCustomerInputModel);
        }
    }
}
