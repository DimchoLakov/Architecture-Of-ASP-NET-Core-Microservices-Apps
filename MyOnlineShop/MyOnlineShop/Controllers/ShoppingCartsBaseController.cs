using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyOnlineShop.Data.Models.ShoppingCarts;
using MyOnlineShop.Helpers;
using static MyOnlineShop.Constants.ShoppingCartConstants;

namespace MyOnlineShop.Controllers
{
    public class ShoppingCartsBaseController : Controller
    {
        protected ShoppingCart ShoppingCart { get; set; }

        protected void SetShoppingCartToSession()
        {
            this.HttpContext
                .Session
                .SetObjectAsJson(CartName, this.ShoppingCart);
        }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ISession session = this.HttpContext.Session;
            var shoppingCart = session.GetObjectFromJson<ShoppingCart>(CartName);
            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart();
            }

            this.ShoppingCart = shoppingCart;
        }
    }
}
