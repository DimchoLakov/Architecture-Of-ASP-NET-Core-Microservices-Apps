using System.Collections.Generic;

namespace MyOnlineShop.Models.Products
{
    public class Category
    {
        public Category()
        {
            this.CategoryProducts = new HashSet<ProductCategory>();
            this.IsActive = true;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public ICollection<ProductCategory> CategoryProducts { get; set; }
    }
}
