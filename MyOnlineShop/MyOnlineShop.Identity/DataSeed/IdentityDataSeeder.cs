using Microsoft.AspNetCore.Identity;
using MyOnlineShop.Common.Constants;
using MyOnlineShop.Common.Services;
using MyOnlineShop.Identity.Data.Models;
using System.Threading.Tasks;

namespace MyOnlineShop.Identity.DataSeed
{
    public class IdentityDataSeeder : IDataSeeder
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public IdentityDataSeeder(
            UserManager<User> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public void SeedData()
        {
            Task
                .Run(async () =>
                {
                    var roleExists = await this.roleManager.RoleExistsAsync(AuthConstants.AdministratorRoleName);
                    if (!roleExists)
                    {
                        var adminRole = new IdentityRole(AuthConstants.AdministratorRoleName);

                        await this.roleManager.CreateAsync(adminRole);
                    }

                    var user = await this.userManager.FindByEmailAsync("admin@mos.com");
                    if (user == null)
                    {
                        var adminUser = new User
                        {
                            UserName = "admin@mos.com",
                            Email = "admin@mos.com"
                        };

                        await userManager.CreateAsync(adminUser, "adminpass12");

                        await userManager.AddToRoleAsync(adminUser, AuthConstants.AdministratorRoleName);
                    }    
                })
                .GetAwaiter()
                .GetResult();
        }
    }
}
