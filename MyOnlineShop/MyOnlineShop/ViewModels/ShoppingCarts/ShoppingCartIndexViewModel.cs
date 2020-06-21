using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MyOnlineShop.ViewModels.ShoppingCarts
{
    public class ShoppingCartIndexViewModel
    {
        public ShoppingCartIndexViewModel()
        {
            this.CartItemViewModels = new List<CartItemIndexViewModel>();
        }

        public string Total => this.CartItemViewModels
            .Select(x => x.ProductPrice * x.Quantity)
            .DefaultIfEmpty(0)
            .Sum()
            .ToString("N2");

        public ICollection<CartItemIndexViewModel> CartItemViewModels { get; set; } 
    }
}
