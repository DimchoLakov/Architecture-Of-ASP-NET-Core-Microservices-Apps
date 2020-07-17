using MyOnlineShop.Common.ViewModels.ShoppingCarts;
using Refit;
using System.Threading.Tasks;

namespace MyOnlineShop.WebMVC.Services.ShoppingCart
{
    public interface IShoppingCartService
    {
        [Get("/ShoppingCart/Count/{userId}")]
        Task<int> GetCartItemsCount(string userId);

        [Get("/ShoppingCart/{userId}")]
        Task<ShoppingCartViewModel> GetShoppingCart(string userId);

        [Post("/ShoppingCart")]
        Task AddToCart([Body]CartItemViewModel cartItemViewModel);

        [Post("/ShoppingCart/Clear/{userId}")]
        Task Clear([Body]string userId);

        [Post("/ShoppingCart/Remove/{productId}/{userId}")]
        Task RemoveItem([Query]int productId, [Query]string userId);
    }
}
