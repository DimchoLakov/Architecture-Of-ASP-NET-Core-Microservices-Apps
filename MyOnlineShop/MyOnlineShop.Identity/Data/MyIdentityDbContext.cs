using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyOnlineShop.Common.Data;
using MyOnlineShop.Common.Data.Configurations;
using MyOnlineShop.Common.Data.Models;
using MyOnlineShop.Identity.Data.Models;
using System.Reflection;

namespace MyOnlineShop.Identity.Data
{
    public class MyIdentityDbContext : IdentityDbContext<User>, IMessageDbContext
    {
        public MyIdentityDbContext(DbContextOptions<MyIdentityDbContext> options)
            : base(options)
        {
        }

        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfiguration(new MessageConfiguration());

            modelBuilder
                .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
