using Microsoft.AspNetCore.Http;
using MyOnlineShop.Common.Constants;
using System;
using System.Security.Claims;

namespace MyOnlineShop.Common.Services
{

    public class CurrentUserService : ICurrentUserService
    {
        private readonly ClaimsPrincipal user;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            this.user = httpContextAccessor.HttpContext?.User;

            if (user == null)
            {
                throw new InvalidOperationException("This request does not have an authenticated user.");
            }

            this.UserId = this.user.FindFirstValue(ClaimTypes.NameIdentifier);
            this.Username = this.user.FindFirstValue(ClaimTypes.Name);
        }

        public string UserId { get; }
        
        public string Username { get; }

        public bool IsAdministrator => this.user.IsInRole(AuthConstants.AdministratorRoleName);
    }
}
