using Microsoft.AspNetCore.Identity;
using MyOnlineShop.Models.Customers;

namespace MyOnlineShop.SeedData
{
    public class IdentityDataInitializer
    {
        private readonly UserManager<Customer> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public IdentityDataInitializer(UserManager<Customer> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public void SeedData()
        {
            SeedRoles();
            SeedUsers();
        }

        private void SeedUsers()
        {
            if (this.userManager.FindByNameAsync("developer@doverfs.com").Result == null)
            {
                var user = new Customer
                {
                    UserName = "dev@developer.com",
                    Email = "dev@developer.com",
                    FirstName = "Dev",
                    LastName = "Developer"
                };

                IdentityResult result = this.userManager.CreateAsync(user, "Password1234").Result;

                if (result.Succeeded)
                {
                    this.userManager.AddToRoleAsync(user, "Administrator").GetAwaiter().GetResult();
                }
            }

        }

        private void SeedRoles()
        {
            if (!this.roleManager.RoleExistsAsync("Administrator").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Administrator"
                };
                this.roleManager.CreateAsync(role).GetAwaiter().GetResult();
            }
            if (!this.roleManager.RoleExistsAsync("User").Result)
            {
                var role = new IdentityRole
                {
                    Name = "User"
                };
                this.roleManager.CreateAsync(role).GetAwaiter().GetResult();
            }
        }
    }
}
