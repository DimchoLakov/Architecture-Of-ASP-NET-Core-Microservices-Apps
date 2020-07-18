using System;
using System.Collections.Generic;

namespace MyOnlineShop.Catalog.Data.Models.Products
{
    public class Product
    {
        public Product()
        {
            this.ProductCategories = new HashSet<ProductCategory>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int StockAvailable { get; set; }

        public double Weight { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public bool IsArchived { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime LastUpdated { get; set; }

        public ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
