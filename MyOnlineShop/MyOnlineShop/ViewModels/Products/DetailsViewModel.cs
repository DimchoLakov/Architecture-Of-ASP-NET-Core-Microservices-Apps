using System.Collections.Generic;

namespace MyOnlineShop.ViewModels.Products
{
    public class DetailsViewModel
    {
        public DetailsViewModel()
        {
            this.ImageViewModels = new List<ProductImageViewModel>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int StockAvailable { get; set; }

        public double Weight { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public int FromPageNumber { get; set; }

        public ProductImageViewModel PrimaryImageViewModel { get; set; }

        public ICollection<ProductImageViewModel> ImageViewModels { get; set; }
    }
}
