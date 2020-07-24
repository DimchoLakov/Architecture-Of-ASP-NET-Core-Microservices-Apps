using Microsoft.EntityFrameworkCore;
using MyOnlineShop.Common.Data.Configurations;
using MyOnlineShop.Common.Data.Models;
using System.Reflection;

namespace MyOnlineShop.Common.Data
{
    public abstract class MessageDbContext : DbContext
    {
        public MessageDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Message> Messages { get; set; }

        protected abstract Assembly ConfigurationsAssembly { get; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .ApplyConfiguration(new MessageConfiguration());

            builder
                .ApplyConfigurationsFromAssembly(this.ConfigurationsAssembly);

            base.OnModelCreating(builder);
        }
    }
}
