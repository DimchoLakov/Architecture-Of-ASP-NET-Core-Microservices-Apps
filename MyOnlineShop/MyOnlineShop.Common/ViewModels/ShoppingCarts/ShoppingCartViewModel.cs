using System.Collections.Generic;
using System.Linq;

namespace MyOnlineShop.Common.ViewModels.ShoppingCarts
{
    public class ShoppingCartViewModel
    {
        public ShoppingCartViewModel()
        {
            this.CartItemViewModels = new List<CartItemViewModel>();
        }

        public string Total => this.CartItemViewModels
            .Select(x => x.ProductPrice * x.Quantity)
            .DefaultIfEmpty(0)
            .Sum()
            .ToString("N2");

        public ICollection<CartItemViewModel> CartItemViewModels { get; set; } 
    }
}
