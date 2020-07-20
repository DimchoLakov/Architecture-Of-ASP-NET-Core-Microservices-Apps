namespace MyOnlineShop.Common.Messages.Catalog
{
    public class ProductUpdatedMessage
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public double Weight { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }
    }
}
