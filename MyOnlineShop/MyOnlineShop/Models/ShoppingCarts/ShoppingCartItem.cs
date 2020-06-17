using MyOnlineShop.Models.Products;
using System;

namespace MyOnlineShop.Models.ShoppingCarts
{
    public class ShoppingCartItem
    {
        public ShoppingCartItem()
        {
            this.DateTimeAdded = DateTime.Now;
        }

        public int Id { get; set; }
        
        public double Price { get; set; }

        public DateTime? DateTimeAdded { get; set; }

        public int Quantity { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
    }
}
