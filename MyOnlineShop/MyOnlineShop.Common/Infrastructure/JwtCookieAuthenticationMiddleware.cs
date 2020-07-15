using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using MyOnlineShop.Common.Constants;
using MyOnlineShop.Common.Services;
using System.Linq;
using System.Threading.Tasks;

namespace MyOnlineShop.Common.Infrastructure
{
    public class JwtCookieAuthenticationMiddleware : IMiddleware
    {
        private readonly ICurrentTokenService currentToken;

        public JwtCookieAuthenticationMiddleware(ICurrentTokenService currentToken)
            => this.currentToken = currentToken;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var token = context.Request.Cookies[AuthConstants.AuthenticationCookieName];

            if (!string.IsNullOrWhiteSpace(token))
            {
                this.currentToken.Set(token.Split().Last());

                context.Request.Headers.Append(AuthConstants.AuthorizationHeaderName, $"{AuthConstants.AuthorizationHeaderValuePrefix} {token}");
            }

            await next.Invoke(context);
        }
    }

    public static class JwtHeaderAuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtHeaderAuthentication(
            this IApplicationBuilder app)
            => app
                .UseMiddleware<JwtCookieAuthenticationMiddleware>()
                .UseAuthentication();
    }
}
