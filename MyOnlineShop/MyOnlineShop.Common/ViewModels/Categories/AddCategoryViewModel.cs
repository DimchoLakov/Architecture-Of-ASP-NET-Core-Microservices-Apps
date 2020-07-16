using System.ComponentModel.DataAnnotations;

namespace MyOnlineShop.Common.ViewModels.Categories
{
    public class AddCategoryViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
