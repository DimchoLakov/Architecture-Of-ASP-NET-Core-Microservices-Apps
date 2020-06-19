using MyOnlineShop.Data.Models.Products;

namespace MyOnlineShop.Data.Models.Orders
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
