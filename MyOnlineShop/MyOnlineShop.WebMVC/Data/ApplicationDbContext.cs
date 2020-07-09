using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyOnlineShop.WebMVC.Data.Configs;
using MyOnlineShop.WebMVC.Data.Models.Categories;
using MyOnlineShop.WebMVC.Data.Models.Customers;
using MyOnlineShop.WebMVC.Data.Models.Galleries;
using MyOnlineShop.WebMVC.Data.Models.Orders;
using MyOnlineShop.WebMVC.Data.Models.Products;
using MyOnlineShop.WebMVC.Data.Models.ShoppingCarts;

namespace MyOnlineShop.WebMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext<Customer>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfiguration(new AddressConfig())
                .ApplyConfiguration(new CategoryConfig())
                .ApplyConfiguration(new CustomerConfig())
                .ApplyConfiguration(new ImageConfig())
                .ApplyConfiguration(new OrderConfig())
                .ApplyConfiguration(new OrderItemConfig())
                .ApplyConfiguration(new ProductCategoryConfig())
                .ApplyConfiguration(new ProductConfig())
                .ApplyConfiguration(new ShoppingCartConfig())
                .ApplyConfiguration(new ShoppingCartItemConfig());

            base.OnModelCreating(modelBuilder);
        }
    }
}
