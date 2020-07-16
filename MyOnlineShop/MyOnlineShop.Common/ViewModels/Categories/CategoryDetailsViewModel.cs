using MyOnlineShop.Common.ViewModels.Products;
using System.Collections.Generic;

namespace MyOnlineShop.Common.ViewModels.Categories
{
    public class CategoryDetailsViewModel
    {
        public CategoryDetailsViewModel()
        {
            this.ProductIndexViewModels = new List<ProductIndexViewModel>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<ProductIndexViewModel> ProductIndexViewModels { get; set; }
    }
}
