﻿using MyOnlineShop.Common.ViewModels.Orders;
using MyOnlineShop.Common.ViewModels.ShoppingCarts;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyOnlineShop.WebMVC.Services.Ordering
{
    public interface IOrderingService
    {
        [Get("/Orders/{userId}")]
        Task<ICollection<OrderIndexViewModel>> GetUserOrders([Query] string userId);

        [Get("/Orders/{id}")]
        Task<OrderDetailsViewModel> Details(int id);

        [Post("/Orders/{userId}/{addressId}")]
        Task PlaceOrder([Query] string userId, [Query] int addressId, [Body] IEnumerable<CartItemViewModel> cartItemViewModels);
    }
}
