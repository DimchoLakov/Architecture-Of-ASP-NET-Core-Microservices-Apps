﻿using Microsoft.EntityFrameworkCore;
using MyOnlineShop.Catalog.Data.Models.Categories;
using MyOnlineShop.Catalog.Data.Models.Customers;
using MyOnlineShop.Catalog.Data.Models.Products;
using System.Reflection;

namespace MyOnlineShop.Ordering.Data
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
            : base(options)
        {
        }

        public DbSet<Address> Addresses { get; set; }
        
        public DbSet<Category> Categories { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }
        
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
