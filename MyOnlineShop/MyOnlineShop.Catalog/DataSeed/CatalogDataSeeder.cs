using MyOnlineShop.Catalog.Data.Models.Categories;
using MyOnlineShop.Catalog.Data.Models.Products;
using MyOnlineShop.Common.Services;
using MyOnlineShop.Ordering.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyOnlineShop.Catalog.DataSeed
{
    public class CatalogDataSeeder : IDataSeeder
    {
        private readonly CatalogDbContext dbContext;

        public CatalogDataSeeder(CatalogDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void SeedData()
        {
            SeedCategories();

            SeedProducts();
        }

        private void SeedCategories()
        {
            if (!this.dbContext.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category
                    {
                        Name = "Cars",
                        IsActive = true
                    },
                    new Category
                    {
                        Name = "Laptops",
                        IsActive = true
                    },
                    new Category
                    {
                        Name = "Phones",
                        IsActive = true
                    },
                    new Category
                    {
                        Name = "Shoes",
                        IsActive = true
                    },
                    new Category
                    {
                        Name = "Watches",
                        IsActive = true
                    }
                };

                this.dbContext
                    .Categories
                    .AddRange(categories);

                this.dbContext
                    .SaveChanges();
            }
        }

        private void SeedProducts()
        {
            if (!this.dbContext.Products.Any())
            {
                var categories = this.dbContext
                .Categories
                .ToList();

                var products = new List<Product>
            {
                new Product
                {
                    DateAdded = DateTime.Now,
                    Description = "Automobili Lamborghini S.p.A. is an Italian brand and manufacturer of luxury sports cars and SUVs based in Sant\'Agata Bolognese. The company is owned by the Volkswagen Group through its subsidiary Audi.",
                    ImageUrl = "https://images.unsplash.com/photo-1526550517342-e086b387edda?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=801&q=80",
                    LastUpdated = DateTime.Now,
                    Name = "Lamborghini Aventador",
                    Price = 200_000,
                    StockAvailable = 10,
                    Weight = 1300,
                    ProductCategories = new List<ProductCategory>
                    {
                        new ProductCategory
                        {
                            Category = categories
                                        .Where(x => x.Name == "Cars")
                                        .FirstOrDefault()
                        }
                    }
                },
                new Product
                {
                    DateAdded = DateTime.Now,
                    Description = "ROG Mothership is a portable Windows 10 Pro powerhouse with an innovative standing design that enhances cooling for its factory overclocked GeForce RTX™ 2080 GPU and 9th Gen Intel® Core™ i9 eight-core CPU.",
                    ImageUrl = "https://images.unsplash.com/photo-1555680202-c86f0e12f086?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=1350&q=80",
                    LastUpdated = DateTime.Now,
                    Name = "ROG Mothership",
                    Price = 3000,
                    StockAvailable = 20,
                    Weight = 3,
                    ProductCategories = new List<ProductCategory>
                    {
                        new ProductCategory
                        {
                            Category = categories
                                        .Where(x => x.Name == "Laptops")
                                        .FirstOrDefault()
                        }
                    }
                },
                new Product
                {
                    DateAdded = DateTime.Now,
                    Description = "The new Galaxy S20 is made for the powerful camera. We've created more memory for your memories, with up to a massive 128GB of internal storage on the Galaxy S20 to keep all your high-res videos and photos.",
                    ImageUrl = "https://images.unsplash.com/photo-1583573636255-6a41ff5523d4?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=1495&q=80",
                    LastUpdated = DateTime.Now,
                    Name = "Samsung S20",
                    Price = 1200,
                    StockAvailable = 30,
                    Weight = 3,
                    ProductCategories = new List<ProductCategory>
                    {
                        new ProductCategory
                        {
                            Category = categories
                                        .Where(x => x.Name == "Phones")
                                        .FirstOrDefault()
                        }
                    }
                },
                new Product
                {
                    DateAdded = DateTime.Now,
                    Description = "Nike, Inc. is an American multinational corporation that is engaged in the design, development, manufacturing, and worldwide marketing and sales of footwear, apparel, equipment, accessories, and services. ",
                    ImageUrl = "https://images.unsplash.com/photo-1557178416-e694c669c944?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=1351&q=80",
                    LastUpdated = DateTime.Now,
                    Name = "Nike",
                    Price = 60,
                    StockAvailable = 80,
                    Weight = 0.3,
                    ProductCategories = new List<ProductCategory>
                    {
                        new ProductCategory
                        {
                            Category = categories
                                        .Where(x => x.Name == "Shoes")
                                        .FirstOrDefault()
                        }
                    }
                },
                new Product
                {
                    DateAdded = DateTime.Now,
                    Description = "Rolex watches are crafted from the finest raw materials and assembled with scrupulous attention to detail.",
                    ImageUrl = "https://images.unsplash.com/photo-1587836374828-4dbafa94cf0e?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=1350&q=80",
                    LastUpdated = DateTime.Now,
                    Name = "Rolex",
                    Price = 10_000,
                    StockAvailable = 100,
                    Weight = 0.07,
                    ProductCategories = new List<ProductCategory>
                    {
                        new ProductCategory
                        {
                            Category = categories
                                        .Where(x => x.Name == "Watches")
                                        .FirstOrDefault()
                        }
                    }
                }
            };

                this.dbContext
                    .AddRange(products);

                this.dbContext
                    .SaveChanges();
            }
        }
    }
}
