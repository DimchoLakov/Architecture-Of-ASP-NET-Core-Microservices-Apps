using Microsoft.EntityFrameworkCore;
using MyOnlineShop.Common.Data;
using MyOnlineShop.Ordering.Data.Models.Orders;
using System.Reflection;

namespace MyOnlineShop.Ordering.Data
{
    public class OrderingDbContext : MessageDbContext
    {
        public OrderingDbContext(DbContextOptions<OrderingDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        protected override Assembly ConfigurationsAssembly => Assembly.GetExecutingAssembly();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
