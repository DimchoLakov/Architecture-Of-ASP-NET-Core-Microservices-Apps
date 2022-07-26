using MassTransit;
using Microsoft.AspNetCore.Identity;
using MyOnlineShop.Common.Constants;
using MyOnlineShop.Common.Data.Models;
using MyOnlineShop.Common.Messages.Identity;
using MyOnlineShop.Common.Services;
using MyOnlineShop.Identity.Data;
using MyOnlineShop.Identity.Data.Models;
using System.Threading.Tasks;

namespace MyOnlineShop.Identity.DataSeed
{
    public class IdentityDataSeeder : IDataSeeder
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly MyIdentityDbContext myIdentityDbContext;
        private readonly IBus bus;

        public IdentityDataSeeder(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            MyIdentityDbContext myIdentityDbContext,
            IBus bus)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.myIdentityDbContext = myIdentityDbContext;
            this.bus = bus;
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

                        var messageData = new UserCreatedMessage
                        {
                            UserId = user.Id,
                            Email = adminUser.Email,
                            FirstName = adminUser.FirstName,
                            LastName = adminUser.LastName
                        };

                        var message = new Message(messageData);

                        await this.myIdentityDbContext
                            .Messages
                            .AddAsync(message);

                        await this.myIdentityDbContext
                            .SaveChangesAsync();

                        await this.bus.Publish(messageData);

                        var msg = await this.myIdentityDbContext
                            .Messages
                            .FindAsync(message.Id);

                        msg.MarkAsPublished();

                        await this.myIdentityDbContext
                            .SaveChangesAsync();
                    }    
                })
                .GetAwaiter()
                .GetResult();
        }
    }
}
