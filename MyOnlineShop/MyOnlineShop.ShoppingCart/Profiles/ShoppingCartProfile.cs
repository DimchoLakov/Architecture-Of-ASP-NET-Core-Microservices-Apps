using AutoMapper;
using MyOnlineShop.Common.Messages.Catalog;
using MyOnlineShop.Common.ViewModels.ShoppingCarts;
using MyOnlineShop.ShoppingCart.Data.Models.ShoppingCarts;

namespace MyOnlineShop.ShoppingCart.Profiles
{
    public class ShoppingCartProfile : Profile
    {
        public ShoppingCartProfile()
        {
            this.CreateMap<CartItemViewModel, ShoppingCartItem>();

            this.CreateMap<ProductUpdatedMessage, ShoppingCartItem>()
                .ForMember(dest => dest.ProductPrice, opts => opts.MapFrom(src => src.Price))
                .ForMember(dest => dest.ProductDescription, opts => opts.MapFrom(src => src.Description))
                .ForMember(dest => dest.ProductImageUrl, opts => opts.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.ProductName, opts => opts.MapFrom(src => src.Name))
                .ForMember(dest => dest.ProductWeight, opts => opts.MapFrom(src => src.Weight))
                .ForMember(dest => dest.DateTimeAdded, opts => opts.Ignore())
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.Price, opts => opts.Ignore())
                .ForMember(dest => dest.Quantity, opts => opts.Ignore())
                .ForMember(dest => dest.ShoppingCartId, opts => opts.Ignore())
                .ForMember(dest => dest.ShoppingCart, opts => opts.Ignore())
                .ForMember(dest => dest.ProductId, opts => opts.Ignore());
        }
    }
}
