using System;

namespace MyOnlineShop.Common.ViewModels.Products
{
    public class ProductDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int StockAvailable { get; set; }

        public double Weight { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public DateTime DateAdded { get; set; }
        
        public DateTime LastUpdated { get; set; }
        
        public bool IsArchived { get; set; }

        public int FromPageNumber { get; set; }
    }
}
