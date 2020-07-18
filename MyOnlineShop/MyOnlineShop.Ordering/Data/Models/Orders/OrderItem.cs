namespace MyOnlineShop.Ordering.Data.Models.Orders
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public double ProductWeight { get; set; }

        public decimal ProductPrice { get; set; }

        public string ProductDescription { get; set; }

        public string ProductImageUrl { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
