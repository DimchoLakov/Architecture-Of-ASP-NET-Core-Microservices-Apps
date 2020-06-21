using System.ComponentModel.DataAnnotations;

namespace MyOnlineShop.ViewModels.Addresses
{
    public class OrderAddressViewModel
    {
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
