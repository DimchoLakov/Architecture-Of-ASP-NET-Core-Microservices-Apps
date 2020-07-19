using MyOnlineShop.Common.ViewModels.Addresses;
using MyOnlineShop.Common.ViewModels.ShoppingCarts;
using Refit;
using System.Threading.Tasks;

namespace MyOnlineShop.ShoppingCart.Gateway.Services.Catalog
{
    public interface ICatalogService
    {
        [Get("/Addresses/{userId}")]
        Task<AddressViewModel> GetAddress(string userId);

        [Post("/Addresses")]
        Task<int> CreateAddress([Query] string userId, OrderAddressViewModel orderAddressViewModel);
    }
}
