namespace MyOnlineShop.Common.ViewModels.Products
{
    public class ProductIndexViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public bool IsArchived { get; set; }
    }
}
