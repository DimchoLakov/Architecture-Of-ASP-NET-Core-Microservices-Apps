using MyOnlineShop.Common.ViewModels.ShoppingCarts;
using Refit;
using System.Threading.Tasks;

namespace MyOnlineShop.ShoppingCart.Gateway.Services.ShoppingCart
{
    public interface IShoppingCartService
    {
        [Get("/ShoppingCart/{userId}")]
        Task<ShoppingCartViewModel> GetShoppingCart(string userId);

        [Post("/ShoppingCart/Clear/{userId}")]
        Task Clear([Body] string userId);
    }
}
