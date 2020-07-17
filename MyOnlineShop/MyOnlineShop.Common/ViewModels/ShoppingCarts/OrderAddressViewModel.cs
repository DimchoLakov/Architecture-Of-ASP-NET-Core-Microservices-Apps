using System.ComponentModel.DataAnnotations;

namespace MyOnlineShop.Common.ViewModels.ShoppingCarts
{
    public class OrderAddressViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Address Line")]
        public string AddressLine { get; set; }

        [Display(Name = "Post Code")]
        public string PostCode { get; set; }

        [Display(Name = "Town")]
        public string Town { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }

        public bool IsAddressAvailable { get; set; }
    }
}
