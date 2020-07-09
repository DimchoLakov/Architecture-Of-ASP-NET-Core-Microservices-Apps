using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MyOnlineShop.WebMVC.Areas.Admin.ViewModels.Images
{
    public class AddImageViewModel
    {
        [Required]
        public string Name { get; set; }

        public bool IsPrimary { get; set; }

        public IFormFile File { get; set; }
    }
}
