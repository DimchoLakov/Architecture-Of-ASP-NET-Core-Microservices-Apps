namespace MyOnlineShop.Common.ViewModels.Addresses
{
    public class AddressViewModel
    {
        public int Id { get; set; }

        public bool IsDeliveryAddress { get; set; }

        public bool IsAddressAvailable { get; set; }

        public string AddressLine { get; set; }

        public string Town { get; set; }

        public string PostCode { get; set; }

        public string Region { get; set; }

        public string Country { get; set; }

        public int CustomerId { get; set; }
    }
}
