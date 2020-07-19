using MyOnlineShop.Common.ViewModels.ShoppingCarts;
using Refit;
using System.Threading.Tasks;

namespace MyOnlineShop.WebMVC.Services.ShoppingCart
{
    public interface IShoppingCartGatewayService
    {
        [Get("/ShoppingCart/Checkout")]
        Task<ShoppingCartOrderWrapperViewModel> GetCheckout();

        [Post("/ShoppingCart/Checkout")]
        Task Checkout([Body]OrderAddressViewModel orderAddressViewModel);
    }
}
