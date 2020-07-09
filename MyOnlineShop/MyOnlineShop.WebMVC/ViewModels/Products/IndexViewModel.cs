namespace MyOnlineShop.WebMVC.ViewModels.Products
{
    public class IndexViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public ProductImageViewModel ImageViewModel { get; set; }
    }
}
