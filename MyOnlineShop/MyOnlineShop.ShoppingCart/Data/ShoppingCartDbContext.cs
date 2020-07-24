using Microsoft.EntityFrameworkCore;
using Cart = MyOnlineShop.ShoppingCart.Data.Models.ShoppingCarts;
using System.Reflection;
using MyOnlineShop.Common.Data;

namespace MyOnlineShop.Ordering.Data
{
    public class ShoppingCartDbContext : MessageDbContext
    {
        public ShoppingCartDbContext(DbContextOptions<ShoppingCartDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Cart.ShoppingCart> ShoppingCarts { get; set; }

        public DbSet<Cart.ShoppingCartItem> ShoppingCartItems { get; set; }

        protected override Assembly ConfigurationsAssembly => Assembly.GetExecutingAssembly();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
