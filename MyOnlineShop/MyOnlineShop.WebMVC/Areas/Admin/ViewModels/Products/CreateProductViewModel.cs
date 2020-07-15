﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyOnlineShop.WebMVC.Areas.Admin.ViewModels.Products
{
    public class CreateProductViewModel
    {
        public CreateProductViewModel()
        {
            this.Files = new List<IFormFile>();
        }

        [Display(Name = "Product Name")]
        [DataType(DataType.Text)]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Stock Available")]
        [Required]
        public int StockAvailable { get; set; }

        [Display(Name = "Weight in Kg")]
        [Required]
        public double Weight { get; set; }

        [Display(Name = "Price $")]
        [DataType(DataType.Currency)]
        [Required]
        public decimal Price { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        [Required]
        public string Description { get; set; }

        [Display(Name = "Category")]
        public int? CategoryId { get; set; }
        public ICollection<SelectListItem> Categories { get; set; }

        [Display(Name = "Images")]
        public ICollection<IFormFile> Files { get; set; }
    }
}