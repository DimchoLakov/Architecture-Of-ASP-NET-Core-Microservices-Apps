namespace MyOnlineShop.Catalog.Data.Models.Customers
{
    public class Address
    {
        public int Id { get; set; }

        public bool IsDeliveryAddress { get; set; }

        public string AddressLine { get; set; }

        public string Town { get; set; }

        public string PostCode { get; set; }

        public string Region { get; set; }

        public string Country { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
