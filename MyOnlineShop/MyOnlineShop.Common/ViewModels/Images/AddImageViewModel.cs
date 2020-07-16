using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MyOnlineShop.Common.ViewModels.Images
{
    public class AddImageViewModel
    {
        [Required]
        public string Name { get; set; }

        public bool IsPrimary { get; set; }

        public IFormFile File { get; set; }
    }
}
