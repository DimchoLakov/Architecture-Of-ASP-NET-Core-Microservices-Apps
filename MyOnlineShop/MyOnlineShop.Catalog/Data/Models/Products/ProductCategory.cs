using MyOnlineShop.Catalog.Data.Models.Categories;

namespace MyOnlineShop.Catalog.Data.Models.Products
{
    public class ProductCategory
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
