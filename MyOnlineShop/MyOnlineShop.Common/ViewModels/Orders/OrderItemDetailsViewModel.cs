namespace MyOnlineShop.Common.ViewModels.Orders
{
    public class OrderItemDetailsViewModel
    {
        public int Quantity { get; set; }

        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }

        public string ProductDescription { get; set; }

        public string PrimaryImageUrl { get; set; }
    }
}
