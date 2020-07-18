using AutoMapper;
using MyOnlineShop.Common.ViewModels.ShoppingCarts;
using MyOnlineShop.ShoppingCart.Data.Models.ShoppingCarts;

namespace MyOnlineShop.ShoppingCart.Profiles
{
    public class ShoppingCartProfile : Profile
    {
        public ShoppingCartProfile()
        {
            this.CreateMap<CartItemViewModel, ShoppingCartItem>();
        }
    }
}
