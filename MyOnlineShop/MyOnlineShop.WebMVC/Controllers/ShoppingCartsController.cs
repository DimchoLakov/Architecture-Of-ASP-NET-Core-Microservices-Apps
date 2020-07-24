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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        private readonly IShoppingCartGatewayService shoppingCartGatewayService;

        public ShoppingCartsController(
            IMapper mapper,
            IShoppingCartService shoppingCartService,
            ICatalogService catalogService,
            IOrderingService orderingService,
            ICurrentUserService currentUserService,
            IShoppingCartGatewayService shoppingCartGatewayService)
        {
            this.mapper = mapper;
            this.shoppingCartService = shoppingCartService;
            this.catalogService = catalogService;
            this.orderingService = orderingService;
            this.currentUserService = currentUserService;
            this.shoppingCartGatewayService = shoppingCartGatewayService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var shoppingCartViewModel = await this.shoppingCartService.GetShoppingCart(this.currentUserService.UserId);

                return this.View(shoppingCartViewModel);
            }
            catch (Refit.ApiException apiEx)
            {
                if (apiEx.HasContent)
                {
                    JsonConvert
                        .DeserializeObject<List<string>>(apiEx.Content)
                        .ForEach(error => this.ModelState.AddModelError(string.Empty, error));
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, ErrorConstants.InternalServerErrorMessage);
                }

                this.HandleException(apiEx);
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

                await this.shoppingCartService.AddToCart(this.currentUserService.UserId, cartItemViewModel);

                return this.Redirect($"/Products/Index/?currentPage={fromPage}");
            }
            catch (Refit.ApiException apiEx)
            {
                if (apiEx.HasContent)
                {
                    JsonConvert
                        .DeserializeObject<List<string>>(apiEx.Content)
                        .ForEach(error => this.ModelState.AddModelError(string.Empty, error));
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, ErrorConstants.InternalServerErrorMessage);
                }

                this.HandleException(apiEx);

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
            catch (Refit.ApiException apiEx)
            {
                if (apiEx.HasContent)
                {
                    JsonConvert
                        .DeserializeObject<List<string>>(apiEx.Content)
                        .ForEach(error => this.ModelState.AddModelError(string.Empty, error));
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, ErrorConstants.InternalServerErrorMessage);
                }

                this.HandleException(apiEx);
            }

            return this.Redirect(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItem(int productId)
        {
            try
            {
                await this.shoppingCartService.RemoveItem(productId, this.currentUserService.UserId);

                return this.Redirect(nameof(Index));
            }
            catch (Refit.ApiException apiEx)
            {
                if (apiEx.HasContent)
                {
                    JsonConvert
                        .DeserializeObject<List<string>>(apiEx.Content)
                        .ForEach(error => this.ModelState.AddModelError(string.Empty, error));
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, ErrorConstants.InternalServerErrorMessage);
                }

                this.HandleException(apiEx);
            }

            return this.Redirect(nameof(Index));
        }

        public async Task<IActionResult> Checkout()
        {
            try
            {
                var shoppingCartOrderWrapperViewModel = await this.shoppingCartGatewayService.GetCheckout();

                return this.View(shoppingCartOrderWrapperViewModel);
            }
            catch (Refit.ApiException apiEx)
            {
                if (apiEx.HasContent)
                {
                    JsonConvert
                        .DeserializeObject<List<string>>(apiEx.Content)
                        .ForEach(error => this.ModelState.AddModelError(string.Empty, error));
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, ErrorConstants.InternalServerErrorMessage);
                }

                this.HandleException(apiEx);
            }

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(OrderAddressViewModel orderAddressViewModel)
        {
            try
            {
                await this.shoppingCartGatewayService.Checkout(orderAddressViewModel);

                return this.RedirectToAction(nameof(Success));
            }
            catch (Refit.ApiException apiEx)
            {
                if (apiEx.HasContent)
                {
                    JsonConvert
                        .DeserializeObject<List<string>>(apiEx.Content)
                        .ForEach(error => this.ModelState.AddModelError(string.Empty, error));
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, ErrorConstants.InternalServerErrorMessage);
                }

                this.HandleException(apiEx);
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
