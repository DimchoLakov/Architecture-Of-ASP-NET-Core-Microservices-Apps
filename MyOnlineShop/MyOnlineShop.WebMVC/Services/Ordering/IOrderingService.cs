using MyOnlineShop.Common.ViewModels.Orders;
using MyOnlineShop.Common.ViewModels.ShoppingCarts;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyOnlineShop.WebMVC.Services.Ordering
{
    public interface IOrderingService
    {
        [Get("/Orders")]
        Task<ICollection<OrderIndexViewModel>> GetUserOrders([Query] string userId);

        [Get("/Orders/{id}")]
        Task<OrderDetailsViewModel> Details(int id);
    }
}
