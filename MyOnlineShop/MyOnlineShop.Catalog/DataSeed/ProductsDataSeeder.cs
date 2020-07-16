using MyOnlineShop.Catalog.Data.Models.Products;
using MyOnlineShop.Common.Services;
using MyOnlineShop.Ordering.Data;
using System;
using System.Collections.Generic;

namespace MyOnlineShop.Catalog.DataSeed
{
    public class ProductsDataSeeder : IDataSeeder
    {
        private readonly CatalogDbContext dbContext;

        public ProductsDataSeeder(CatalogDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void SeedData()
        {
            var products = new List<Product>();

            for (int i = 0; i < 10; i++)
            {
                var product = new Product
                {
                    DateAdded = DateTime.Now.AddDays(-i),
                    Description = $"Description {i}",
                    Name = $"Name {i}",
                    Price = i + 1 * 2,
                    LastUpdated = DateTime.Now.AddDays(-i),
                    Weight = i + 1,
                    StockAvailable = 3
                };

                products.Add(product);
            }

            this.dbContext
                .AddRange(products);

            this.dbContext
                .SaveChanges();
        }
    }
}
