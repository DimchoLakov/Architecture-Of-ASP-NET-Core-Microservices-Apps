using MyOnlineShop.Common.ViewModels.ShoppingCarts;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyOnlineShop.ShoppingCart.Gateway.Services.Ordering
{
    public interface IOrderingService
    {
        [Post("/Orders")]
        Task PlaceOrder([Query] string userId, [Query] int addressId, [Body] IEnumerable<CartItemViewModel> cartItemViewModels);
    }
}
