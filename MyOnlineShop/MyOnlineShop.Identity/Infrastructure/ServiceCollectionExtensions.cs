using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MyOnlineShop.Identity.Data;
using MyOnlineShop.Identity.Data.Models;

namespace MyOnlineShop.Identity.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUserStorage(this IServiceCollection services)
        {
            services
                .AddIdentity<User, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<MyIdentityDbContext>();

            return services;
        }
    }
}
