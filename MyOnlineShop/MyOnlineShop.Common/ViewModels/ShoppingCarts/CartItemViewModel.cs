using System.ComponentModel.DataAnnotations;

namespace MyOnlineShop.Common.ViewModels.ShoppingCarts
{
    public class CartItemViewModel
    {
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        public double ProductWeight { get; set; }

        public decimal ProductPrice { get; set; }

        public string ProductDescription { get; set; }

        public string ProductImageUrl { get; set; }

        public int Quantity { get; set; }

        public string Total => (this.ProductPrice * this.Quantity).ToString("N2");
    }
}
