using MyOnlineShop.Areas.Admin.ViewModels.Pagination;
using System.Collections.Generic;

namespace MyOnlineShop.Areas.Admin.ViewModels.Products
{
    public class ProductPaginationViewModel
    {
        public ProductPaginationViewModel()
        {
            this.ProductIndexViewModels = new List<ProductIndexViewModel>();
        }

        public string Search { get; set; }

        public PaginationViewModel PaginationViewModel { get; set; }

        public IList<ProductIndexViewModel> ProductIndexViewModels { get; set; }
    }
}
