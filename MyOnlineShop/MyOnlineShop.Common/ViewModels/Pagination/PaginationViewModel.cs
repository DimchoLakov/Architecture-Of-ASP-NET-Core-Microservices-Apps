using System.ComponentModel.DataAnnotations;

namespace MyOnlineShop.Common.ViewModels.Pagination
{
    public class PaginationViewModel
    {
        [Display(Name = "Current Page")]
        public int CurrentPage { get; set; }

        [Display(Name = "Total Pages")]
        public int TotalPages { get; set; }

        [Display(Name = "Search")]
        public string Search { get; set; }
    }
}
