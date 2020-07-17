using Microsoft.EntityFrameworkCore;
using Cart = MyOnlineShop.ShoppingCart.Data.Models.ShoppingCarts;
using System.Reflection;

namespace MyOnlineShop.Ordering.Data
{
    public class ShoppingCartDbContext : DbContext
    {
        public ShoppingCartDbContext(DbContextOptions<ShoppingCartDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Cart.ShoppingCart> ShoppingCarts { get; set; }

        public DbSet<Cart.ShoppingCartItem> ShoppingCartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
