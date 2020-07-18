using Microsoft.AspNetCore.Mvc;
using MyOnlineShop.Common.Services;
using MyOnlineShop.WebMVC.Services.ShoppingCart;
using System;
using System.Threading.Tasks;

namespace MyOnlineShop.WebMVC.ViewComponents
{
    public class CartItemsCountViewComponent : ViewComponent
    {
        private readonly IShoppingCartService shoppingCartService;
        private readonly ICurrentUserService currentUserService;

        public CartItemsCountViewComponent(
            IShoppingCartService shoppingCartService,
            ICurrentUserService currentUserService)
        {
            this.shoppingCartService = shoppingCartService;
            this.currentUserService = currentUserService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var cartItemsCount = await this.shoppingCartService.GetCartItemsCount(this.currentUserService.UserId);

                ViewBag.IsCartInoperative = false;

                return this.View(cartItemsCount);
            }
            catch(Exception)
            {
                ViewBag.IsCartInoperative = true;

                return this.View(0);
            }
        }
    }
}
