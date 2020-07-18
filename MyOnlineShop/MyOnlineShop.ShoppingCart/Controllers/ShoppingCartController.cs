using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyOnlineShop.Common.Controllers;
using MyOnlineShop.Common.ViewModels.ShoppingCarts;
using MyOnlineShop.Ordering.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MyOnlineShop.ShoppingCart.Controllers
{
    [Authorize]
    public class ShoppingCartController : ApiController
    {
        private readonly ShoppingCartDbContext shoppingCartDbContext;

        public ShoppingCartController(ShoppingCartDbContext shoppingCartDbContext)
        {
            this.shoppingCartDbContext = shoppingCartDbContext;
        }

        [HttpGet]
        [Route("Count" + PathSeparator + Id)]
        public async Task<ActionResult<int>> GetCartItemsCount(string userId)
        {
            return await this.shoppingCartDbContext
                .ShoppingCartItems
                .Where(x => x.ShoppingCart.UserId == userId)
                .CountAsync();
        }

        [HttpGet]
        [Route(Id)]
        public async Task<ActionResult<ShoppingCartViewModel>> Get(string userId)
        {
            var shoppingCartViewModel = await this.shoppingCartDbContext
                .ShoppingCarts
                .Where(x => x.UserId == userId)
                .Select(x => new ShoppingCartViewModel
                {
                    CartItemViewModels = x
                                          .CartItems
                                          .Select(ci => new CartItemViewModel
                                          {
                                              ProductId = ci.ProductId,
                                              ProductName = ci.ProductName,
                                              ProductPrice = ci.ProductPrice,
                                              ProductWeight = ci.ProductWeight,
                                              Quantity = ci.Quantity
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
    }
}
