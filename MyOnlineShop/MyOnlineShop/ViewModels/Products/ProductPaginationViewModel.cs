using MyOnlineShop.ViewModels.Pagination;
using System.Collections.Generic;

namespace MyOnlineShop.ViewModels.Products
{
    public class ProductPaginationViewModel
    {
        public ProductPaginationViewModel()
        {
            this.ProductIndexViewModels = new List<IndexViewModel>();
        }

        public string Search { get; set; }

        public PaginationViewModel PaginationViewModel { get; set; }

        public IList<IndexViewModel> ProductIndexViewModels { get; set; }
    }
}
