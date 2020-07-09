using System.Globalization;

namespace MyOnlineShop.WebMVC.ViewModels.ShoppingCarts
{
    public class CartItemIndexViewModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }
        
        public int Quantity { get; set; }

        public string Total => (this.ProductPrice * this.Quantity).ToString("N2");
    }
}
