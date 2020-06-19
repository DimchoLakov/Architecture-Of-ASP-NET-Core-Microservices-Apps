using System.ComponentModel.DataAnnotations;

namespace MyOnlineShop.Areas.Admin.ViewModels.Categories
{
    public class AddCategoryViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
