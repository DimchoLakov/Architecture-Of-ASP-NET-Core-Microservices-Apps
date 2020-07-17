using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyOnlineShop.Common.Constants;
using MyOnlineShop.Common.Services;
using MyOnlineShop.Common.ViewModels.Addresses;
using MyOnlineShop.Common.ViewModels.Products;
using MyOnlineShop.Common.ViewModels.ShoppingCarts;
using MyOnlineShop.WebMVC.Services.Catalog;
using MyOnlineShop.WebMVC.Services.Ordering;
using MyOnlineShop.WebMVC.Services.ShoppingCart;
using System;
using System.Threading.Tasks;

namespace MyOnlineShop.WebMVC.Controllers
{
    [Authorize]
    public class ShoppingCartsController : Controller
    {
        private readonly IMapper mapper;
        private readonly IShoppingCartService shoppingCartService;
        private readonly ICatalogService catalogService;
        private readonly IOrderingService orderingService;
        private readonly ICurrentUserService currentUserService;

        public ShoppingCartsController(
            IMapper mapper,
            IShoppingCartService shoppingCartService,
            ICatalogService catalogService,
            IOrderingService orderingService,
            ICurrentUserService currentUserService)
        {
            this.mapper = mapper;
            this.shoppingCartService = shoppingCartService;
            this.catalogService = catalogService;
            this.orderingService = orderingService;
            this.currentUserService = currentUserService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var shoppingCartViewModel = await this.shoppingCartService.GetShoppingCart(this.currentUserService.UserId);

                return this.View(shoppingCartViewModel);
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(int productId, int? fromPage = 1)
        {
            try
            {
                var productDetailsViewModel = await this.catalogService.GetProductDetails(productId, fromPage);

                var cartItemViewModel = this.mapper.Map<ProductDetailsViewModel, CartItemViewModel>(productDetailsViewModel);

                await this.shoppingCartService.AddToCart(cartItemViewModel);

                return this.Redirect($"/Products/Index/?currentPage={fromPage}");
            }
            catch (Exception ex)
            {
                this.HandleException(ex);

                return this.Redirect($"/Products/Index/?currentPage={fromPage}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            try
            {
                await this.shoppingCartService.Clear(this.currentUserService.UserId);

                return this.Redirect(nameof(Index));
            }
            catch (Exception ex)
            {
                this.HandleException(ex);

                return this.Redirect(nameof(Index));
            }
        }

        [HttpPost]
        public IActionResult RemoveItem(int productId)
        {
            try
            {
                this.shoppingCartService.RemoveItem(productId, this.currentUserService.UserId);

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                this.HandleException(ex);

                return this.RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Checkout()
        {
            try
            {
                var shoppingCartOrderWrapperViewModel = new ShoppingCartOrderWrapperViewModel
                {
                    OrderAddressViewModel = this.mapper.Map<AddressViewModel, OrderAddressViewModel>(
                                                       await this.catalogService.GetAddress(this.currentUserService.UserId)),
                    ShoppingCartViewModel = await this.shoppingCartService.GetShoppingCart(this.currentUserService.UserId)
                };

                return this.View(shoppingCartOrderWrapperViewModel);
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(OrderAddressViewModel orderAddressViewModel)
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

                return this.RedirectToAction(nameof(Success));
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return this.View();
        }

        public IActionResult Success()
        {
            return this.View("Success", ShoppingCartConstants.SuccessMessage);
        }

        private void HandleException(Exception ex)
        {
            ViewBag.ShoppingCartInoperativeMsg = $"Shopping Cart Service is inoperative {ex.GetType().Name} - {ex.Message}";
        }
    }
}
