using Microsoft.AspNetCore.Mvc.Rendering;
using MyOnlineShop.Areas.Admin.ViewModels.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyOnlineShop.Areas.Admin.ViewModels.Products
{
    public class CreateProductViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int StockAvailable { get; set; }

        [Required]
        public double Weight { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public ICollection<SelectListItem> Categories { get; set; }

        public IFormFileWrapper FileWrapper { get; set; }
    }
}
