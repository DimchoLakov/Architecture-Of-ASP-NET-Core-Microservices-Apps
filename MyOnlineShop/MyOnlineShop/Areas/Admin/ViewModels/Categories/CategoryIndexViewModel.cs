using MyOnlineShop.Areas.Admin.ViewModels.Products;
using System.Collections.Generic;

namespace MyOnlineShop.Areas.Admin.ViewModels.Categories
{
    public class CategoryIndexViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
