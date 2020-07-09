using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyOnlineShop.WebMVC.Data;
using MyOnlineShop.WebMVC.Data.Models.Customers;
using MyOnlineShop.WebMVC.Data.Models.Orders;
using MyOnlineShop.WebMVC.Data.Models.ShoppingCarts;
using MyOnlineShop.WebMVC.ViewModels.Addresses;
using MyOnlineShop.WebMVC.ViewModels.ShoppingCarts;
using System;
using System.Linq;
using System.Threading.Tasks;
using static MyOnlineShop.WebMVC.Constants.ProductConstants;
using static MyOnlineShop.WebMVC.Constants.ShoppingCartConstants;

namespace MyOnlineShop.WebMVC.Controllers
{
    public class ShoppingCartsController : ShoppingCartsBaseController
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public ShoppingCartsController(
            ApplicationDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            int[] productIds = this.ShoppingCart
                .CartItems
                .Select(x => x.ProductId)
                .ToArray();

            var cartItemIndexViewModels = await this.dbContext
                .Products
                .Where(x => productIds.Contains(x.Id))
                .Select(x => new CartItemIndexViewModel
                {
                    ProductId = x.Id,
                    ProductName = x.Name,
                    ProductPrice = x.Price
                })
                .ToListAsync();

            foreach (var cartItemIndexViewModel in cartItemIndexViewModels)
            {
                var cartItem = this.ShoppingCart
                    .CartItems
                    .FirstOrDefault(x => x.ProductId == cartItemIndexViewModel.ProductId);
                if (cartItem != null)
                {
                    cartItemIndexViewModel.Quantity = cartItem.Quantity;
                }
            }

            var shoppingCartIndexViewModel = new ShoppingCartIndexViewModel 
            {
                 CartItemViewModels = cartItemIndexViewModels
            };

            return this.View(shoppingCartIndexViewModel);
        }

        public async Task<IActionResult> AddProduct(int productId, int? fromPage = 1)
        {
            var productExists = await this.dbContext
                .Products
                .AnyAsync(x => x.Id == productId);

            if (!productExists)
            {
                return this.BadRequest(ProductDoesNotExistMessage);
            }

            decimal productPrice = await this.dbContext
                .Products
                .Where(x => x.Id == productId)
                .Select(x => x.Price)
                .FirstOrDefaultAsync();

            ShoppingCartItem cartItem = this.ShoppingCart
                .CartItems
                .FirstOrDefault(x => x.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                this.ShoppingCart.CartItems.Add(new ShoppingCartItem
                {
                    DateTimeAdded = DateTime.UtcNow,
                    ProductId = productId,
                    Price = productPrice,
                    Quantity = 1
                });
            }

            this.SetShoppingCartToSession();

            return this.Redirect($"/Products/Index/?currentPage={fromPage}");
        }

        [HttpPost]
        public IActionResult Clear()
        {
            this.HttpContext.Session.Clear();

            return this.RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult RemoveItem(int productId)
        {
            ShoppingCartItem cartItem = this.ShoppingCart
                .CartItems
                .FirstOrDefault(x => x.ProductId == productId);

            if (cartItem != null)
            {
                this.ShoppingCart
                    .CartItems
                    .Remove(cartItem);
            }

            this.SetShoppingCartToSession();

            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Checkout()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return this.Redirect("/Identity/Login/?returnUrl=/ShoppingCarts/Checkout/");
            }

            int[] productIds = this.ShoppingCart
                .CartItems
                .Select(x => x.ProductId)
                .ToArray();

            var cartItemIndexViewModels = await this.dbContext
                .Products
                .Where(x => productIds.Contains(x.Id))
                .Select(x => new CartItemIndexViewModel
                {
                    ProductId = x.Id,
                    ProductName = x.Name,
                    ProductPrice = x.Price
                })
                .ToListAsync();

            foreach (var cartItemIndexViewModel in cartItemIndexViewModels)
            {
                var cartItem = this.ShoppingCart
                    .CartItems
                    .FirstOrDefault(x => x.ProductId == cartItemIndexViewModel.ProductId);
                if (cartItem != null)
                {
                    cartItemIndexViewModel.Quantity = cartItem.Quantity;
                }
            }

            var currentUser = await this.dbContext
                .Users
                .Include(x => x.Addresses)
                .FirstOrDefaultAsync(x => x.Email == this.User.Identity.Name);

            var isAddressAvailable = currentUser
                                            .Addresses
                                            .Any();

            var orderAddressViewModel = new OrderAddressViewModel();
            if (isAddressAvailable)
            {
                var currentDeliveryAddress = currentUser
                    .Addresses
                    .FirstOrDefault(x => x.IsDeliveryAddress);

                if (currentDeliveryAddress != null)
                {
                    orderAddressViewModel = this.mapper.Map<Address, OrderAddressViewModel>(currentDeliveryAddress);
                    orderAddressViewModel.IsAddressAvailable = true;
                }
                else
                {
                    currentDeliveryAddress = currentUser
                    .Addresses
                    .FirstOrDefault();

                    if (currentDeliveryAddress != null)
                    {
                        orderAddressViewModel = this.mapper.Map<Address, OrderAddressViewModel>(currentDeliveryAddress);
                        orderAddressViewModel.IsAddressAvailable = true;
                    }
                }
            }

            var shoppingCartOrderAddressViewModel = new ShoppingCartOrderAddressViewModel
            {
                ShoppingCartIndexViewModel = new ShoppingCartIndexViewModel
                {
                    CartItemViewModels = cartItemIndexViewModels
                },
                OrderAddressViewModel = orderAddressViewModel,
            };

            return this.View(shoppingCartOrderAddressViewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Checkout(OrderAddressViewModel orderAddressViewModel)
        {
            if (this.ShoppingCart == null)
            {
                return this.BadRequest();
            }

            var currentUser = await this.dbContext
                .Users
                .Include(x => x.Addresses)
                .FirstOrDefaultAsync(x => x.Email == this.User.Identity.Name);

            var random = new Random();
            var order = new Order
            {
                DeliveryCost = random.Next(1, 6),
                Date = DateTime.UtcNow,
                Customer = currentUser
            };
            if (!currentUser.Addresses.Any())
            {
                var newAddress = new Address
                {
                    AddressLine = orderAddressViewModel.AddressLine,
                    Country = orderAddressViewModel.Country,
                    PostCode = orderAddressViewModel.PostCode,
                    Town = orderAddressViewModel.Town,
                    IsDeliveryAddress = true
                };
                order.DeliveryAddress = newAddress;

                currentUser
                    .Addresses
                    .Add(newAddress);
            }
            else
            {
                var deliveryAddress = await this.dbContext
                    .Customers
                    .Where(x => x.Email == this.User.Identity.Name)
                    .SelectMany(x => x.Addresses)
                    .Where(x => x.IsDeliveryAddress)
                    .FirstOrDefaultAsync();

                if (deliveryAddress != null)
                {
                    order.DeliveryAddress = deliveryAddress;
                }
                else
                {
                    var address = await this.dbContext
                    .Customers
                    .Where(x => x.Email == this.User.Identity.Name)
                    .SelectMany(x => x.Addresses)
                    .FirstOrDefaultAsync();

                    order.DeliveryAddress = address;
                }
            }

            foreach (ShoppingCartItem cartItem in this.ShoppingCart.CartItems)
            {
                order.OrderItems.Add(new OrderItem
                {
                    Price = cartItem.Price,
                    Quantity = cartItem.Quantity,
                    ProductId = cartItem.ProductId
                });
            }

            this.dbContext
                .Users
                .Update(currentUser);

            await this.dbContext
                .Orders
                .AddAsync(order);

            await this.dbContext
                .SaveChangesAsync();

            this.HttpContext.Session.Clear();

            return this.RedirectToAction(nameof(Success));
        }

        public IActionResult Success()
        {
            return this.View("Success", SuccessMessage);
        }
    }
}
