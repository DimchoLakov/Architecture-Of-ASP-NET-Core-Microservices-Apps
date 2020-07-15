﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyOnlineShop.WebMVC.Areas.Admin.ViewModels.Products
{
    public class EditProductViewModel
    {
        public EditProductViewModel()
        {
            this.ImageViewModels = new List<ProductImageViewModel>();
        }

        public int Id { get; set; }

        [Display(Name = "Product Name: ")]
        [DataType(DataType.Text)]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Stock Available: ")]
        [Required]
        public int StockAvailable { get; set; }

        [Display(Name = "Weight in Kg: ")]
        [Required]
        public double Weight { get; set; }

        [Display(Name = "Price: $")]
        [DataType(DataType.Currency)]
        [Required]
        public decimal Price { get; set; }

        [Display(Name = "Description: ")]
        [DataType(DataType.MultilineText)]
        [Required]
        public string Description { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime LastUpdated { get; set; }

        public bool IsArchived { get; set; }

        public int FromPageNumber { get; set; }

        public ProductImageViewModel PrimaryImageViewModel { get; set; }

        public ICollection<ProductImageViewModel> ImageViewModels { get; set; }
    }
}