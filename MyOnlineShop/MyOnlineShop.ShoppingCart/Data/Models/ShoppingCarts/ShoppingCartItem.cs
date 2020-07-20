using System;

namespace MyOnlineShop.ShoppingCart.Data.Models.ShoppingCarts
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

        public string ProductName { get; set; }

        public double ProductWeight { get; set; }

        public decimal ProductPrice { get; set; }
        
        public string ProductDescription { get; set; }

        public string ProductImageUrl { get; set; }

        public bool IsArchived { get; set; }

        public string ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
    }
}
