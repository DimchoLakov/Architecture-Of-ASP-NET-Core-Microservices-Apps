using Microsoft.AspNetCore.Mvc;
using MyOnlineShop.WebMVC.Data;
using MyOnlineShop.WebMVC.Data.Models.ShoppingCarts;
using MyOnlineShop.WebMVC.Helpers;
using System.Linq;
using System.Threading.Tasks;
using static MyOnlineShop.WebMVC.Constants.ShoppingCartConstants;

namespace MyOnlineShop.WebMVC.ViewComponents
{
    public class CartItemsCountViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext dbContext;

        public CartItemsCountViewComponent(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int cartItemsCount = 0;
            var session = this.HttpContext.Session;
            if (session.IsAvailable)
            {
                var shoppingCart = session.GetObjectFromJson<ShoppingCart>(CartName);
                if (shoppingCart != null)
                {
                    cartItemsCount = shoppingCart
                        .CartItems
                        .Select(x => x.Quantity)
                        .DefaultIfEmpty(0)
                        .Sum();
                }
            }

            return await Task.Run(() => this.View(cartItemsCount));
        }
    }
}
