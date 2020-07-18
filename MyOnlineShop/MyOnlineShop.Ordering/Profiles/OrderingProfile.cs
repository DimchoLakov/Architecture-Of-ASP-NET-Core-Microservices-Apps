using AutoMapper;
using MyOnlineShop.Common.ViewModels.ShoppingCarts;
using MyOnlineShop.Ordering.Data.Models.Orders;

namespace MyOnlineShop.Ordering.Profiles
{
    public class OrderingProfile : Profile
    {
        public OrderingProfile()
        {
            this.CreateMap<CartItemViewModel, OrderItem>()
                .ForMember(dest => dest.Price, opts => opts.MapFrom(src => src.Total));
        }
    }
}
