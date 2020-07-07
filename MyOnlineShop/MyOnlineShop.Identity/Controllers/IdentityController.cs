using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyOnlineShop.Identity.Services.Identity;
using MyOnlineShop.Identity.Models;
using System.Threading.Tasks;

using static MyOnlineShop.Common.Constants.Constants;

namespace MyOnlineShop.Identity.Controllers
{
    [Authorize(Roles = AdministratorRoleName)]
    public class IdentityController : ApiController
    {
        private readonly IIdentityService identityService;

        public IdentityController(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Login(LoginCustomerInputModel loginCustomerInputModel)
        {
            var result = await this.identityService.LoginAsync(loginCustomerInputModel);

            if (!result.Succeeded)
            {
                return this.BadRequest(result.Errors);
            }

            return result.Token;
        }

        [HttpPost]
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
