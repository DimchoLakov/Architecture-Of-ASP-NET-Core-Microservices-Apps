﻿using System.Collections.Generic;

namespace MyOnlineShop.Models.ShoppingCarts
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            this.CartItems = new HashSet<ShoppingCartItem>();
        }

        public string Id { get; set; }

        public ICollection<ShoppingCartItem> CartItems { get; set; }
    }
}
