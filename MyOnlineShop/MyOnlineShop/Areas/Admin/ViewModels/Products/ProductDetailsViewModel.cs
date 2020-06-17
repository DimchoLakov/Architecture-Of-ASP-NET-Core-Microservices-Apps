using System.Collections.Generic;

namespace MyOnlineShop.Areas.Admin.ViewModels.Products
{
    public class ProductDetailsViewModel
    {
        public ProductDetailsViewModel()
        {
            this.ImageViewModels = new List<ProductImageViewModel>();
        }

        public string Name { get; set; }

        public int StockAvailable { get; set; }

        public double Weight { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public ICollection<ProductImageViewModel> ImageViewModels { get; set; }
    }
}
