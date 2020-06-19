using MyOnlineShop.Data.Models.Categories;

namespace MyOnlineShop.Data.Models.Products
{
    public class ProductCategory
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
