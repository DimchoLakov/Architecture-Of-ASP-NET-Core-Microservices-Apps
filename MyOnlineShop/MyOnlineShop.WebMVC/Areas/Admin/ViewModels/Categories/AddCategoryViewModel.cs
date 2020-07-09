using System.ComponentModel.DataAnnotations;

namespace MyOnlineShop.WebMVC.Areas.Admin.ViewModels.Categories
{
    public class AddCategoryViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
