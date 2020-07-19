using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyOnlineShop.Common.Controllers;
using MyOnlineShop.Common.Services;
using MyOnlineShop.Common.ViewModels.Addresses;
using MyOnlineShop.Common.ViewModels.ShoppingCarts;
using MyOnlineShop.ShoppingCart.Gateway.Services.Catalog;
using MyOnlineShop.ShoppingCart.Gateway.Services.Ordering;
using MyOnlineShop.ShoppingCart.Gateway.Services.ShoppingCart;
using System;
using System.Threading.Tasks;

namespace MyOnlineShop.ShoppingCart.Gateway.Controllers
{
    [Authorize]
    public class ShoppingCartController : ApiController
    {
        private readonly ICatalogService catalogService;
        private readonly IOrderingService orderingService;
        private readonly IShoppingCartService shoppingCartService;
        private readonly ICurrentUserService currentUserService;
        private readonly IMapper mapper;

        public ShoppingCartController(
            ICatalogService catalogService,
            IOrderingService orderingService,
            IShoppingCartService shoppingCartService,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            this.catalogService = catalogService;
            this.orderingService = orderingService;
            this.shoppingCartService = shoppingCartService;
            this.currentUserService = currentUserService;
            this.mapper = mapper;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<ShoppingCartOrderWrapperViewModel>> Checkout()
        {
            try
            {
                var shoppingCartOrderWrapperViewModel = new ShoppingCartOrderWrapperViewModel
                {
                    OrderAddressViewModel = this.mapper.Map<AddressViewModel, OrderAddressViewModel>(
                                                       await this.catalogService.GetAddress(this.currentUserService.UserId)),
                    ShoppingCartViewModel = await this.shoppingCartService.GetShoppingCart(this.currentUserService.UserId)
                };

                return this.Ok(shoppingCartOrderWrapperViewModel);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Checkout(OrderAddressViewModel orderAddressViewModel)
        {
            try
            {
                var shoppingCartViewModel = await this.shoppingCartService.GetShoppingCart(this.currentUserService.UserId);

                if (orderAddressViewModel.IsAddressAvailable)
                {
                    await this.orderingService.PlaceOrder(this.currentUserService.UserId, orderAddressViewModel.Id, shoppingCartViewModel.CartItemViewModels);
                }
                else
                {
                    var addressId = await this.catalogService.CreateAddress(this.currentUserService.UserId, orderAddressViewModel);

                    await this.orderingService.PlaceOrder(this.currentUserService.UserId, addressId, shoppingCartViewModel.CartItemViewModels);
                }

                await this.shoppingCartService.Clear(this.currentUserService.UserId);

                return this.Ok();
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}
