using MyOnlineShop.Areas.Admin.ViewModels.Products;
using System.Collections.Generic;

namespace MyOnlineShop.Areas.Admin.ViewModels.Categories
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
