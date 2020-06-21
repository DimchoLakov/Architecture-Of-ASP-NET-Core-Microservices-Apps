using MyOnlineShop.Data.Models.Products;
using System;

namespace MyOnlineShop.Data.Models.ShoppingCarts
{
    public class ShoppingCartItem
    {
        public ShoppingCartItem()
        {
            this.DateTimeAdded = DateTime.Now;
        }

        public int Id { get; set; }
        
        public decimal Price { get; set; }

        public DateTime? DateTimeAdded { get; set; }

        public int Quantity { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
    }
}
