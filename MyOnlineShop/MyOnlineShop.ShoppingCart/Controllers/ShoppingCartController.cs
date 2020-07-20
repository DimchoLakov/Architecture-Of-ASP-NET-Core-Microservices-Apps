using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyOnlineShop.Common.Constants;
using MyOnlineShop.Common.Controllers;
using MyOnlineShop.Common.ViewModels.ShoppingCarts;
using MyOnlineShop.Ordering.Data;
using MyOnlineShop.ShoppingCart.Data.Models.ShoppingCarts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyOnlineShop.ShoppingCart.Controllers
{
    [Authorize]
    public class ShoppingCartController : ApiController
    {
        private readonly ShoppingCartDbContext shoppingCartDbContext;
        private readonly IMapper mapper;

        public ShoppingCartController(
            ShoppingCartDbContext shoppingCartDbContext,
            IMapper mapper)
        {
            this.shoppingCartDbContext = shoppingCartDbContext;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("Count" + PathSeparator + "{userId}")]
        public async Task<ActionResult<int>> GetCartItemsCount(string userId)
        {
            return await this.shoppingCartDbContext
                .ShoppingCartItems
                .Include(x => x.ShoppingCart)
                .Where(x => x.ShoppingCart.UserId == userId &&
                            !x.IsArchived)
                .CountAsync();
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<ActionResult<ShoppingCartViewModel>> Get(string userId)
        {
            var shoppingCartViewModel = await this.shoppingCartDbContext
                .ShoppingCarts
                .Where(x => x.UserId == userId)
                .Select(x => new ShoppingCartViewModel
                {
                    CartItemViewModels = x
                                          .CartItems
                                          .Where(x => !x.IsArchived)
                                          .Select(ci => new CartItemViewModel
                                          {
                                              ProductId = ci.ProductId,
                                              ProductName = ci.ProductName,
                                              ProductPrice = ci.ProductPrice,
                                              ProductImageUrl = ci.ProductImageUrl,
                                              ProductWeight = ci.ProductWeight,
                                              Quantity = ci.Quantity,
                                              ProductDescription = ci.ProductDescription
                                          })
                                          .ToList()
                })
                .FirstOrDefaultAsync();

            if (shoppingCartViewModel == null)
            {
                return this.Ok(new ShoppingCartViewModel());
            }

            return this.Ok(shoppingCartViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> AddToCart([FromQuery] string userId, CartItemViewModel cartItemViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(ShoppingCartConstants.InvalidCartItemMessage);
            }

            var shoppingCart = await this.shoppingCartDbContext
                .ShoppingCarts
                .Include(x => x.CartItems)
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();

            if (shoppingCart == null)
            {
                var newShoppingCart = new Data.Models.ShoppingCarts.ShoppingCart
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId
                };

                await this.shoppingCartDbContext
                    .ShoppingCarts
                    .AddAsync(newShoppingCart);

                await this.shoppingCartDbContext
                    .SaveChangesAsync();

                shoppingCart = await this.shoppingCartDbContext
                .ShoppingCarts
                .Include(x => x.CartItems)
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();
            }

            var cartItem = shoppingCart
                .CartItems
                .FirstOrDefault(x => x.ProductId == cartItemViewModel.ProductId);

            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                var shoppingCartItem = this.mapper.Map<CartItemViewModel, ShoppingCartItem>(cartItemViewModel);
                shoppingCart.CartItems.Add(shoppingCartItem);
            }

            this.shoppingCartDbContext
                .ShoppingCarts
                .Update(shoppingCart);

            await this.shoppingCartDbContext
                .SaveChangesAsync();

            return this.Ok();
        }

        [HttpPost("Clear/{userId}")]
        public async Task<ActionResult> Clear (string userId)
        {
            var shoppingCart = await this.shoppingCartDbContext
                .ShoppingCarts
                .Include(x => x.CartItems)
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();

            if (shoppingCart != null)
            {
                this.shoppingCartDbContext
                    .ShoppingCartItems
                    .RemoveRange(shoppingCart.CartItems);

                await this.shoppingCartDbContext
                    .SaveChangesAsync();
            }

            return this.Ok();
        }

        [HttpPost("Remove/{productId}/{userId}")]
        public async Task<ActionResult> RemoveItem(int productId, string userId)
        {
            var shoppingCartItem = await this.shoppingCartDbContext
                .ShoppingCartItems
                .Include(x => x.ShoppingCart)
                .FirstOrDefaultAsync(x => x.ProductId == productId &&
                                          x.ShoppingCart.UserId == userId);

            if (shoppingCartItem != null)
            {
                this.shoppingCartDbContext
                    .ShoppingCartItems
                    .Remove(shoppingCartItem);

                await this.shoppingCartDbContext
                    .SaveChangesAsync();
            }

            return this.Ok();
        }
    }
}
