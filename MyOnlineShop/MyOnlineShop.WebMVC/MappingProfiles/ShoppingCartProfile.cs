using AutoMapper;
using MyOnlineShop.Common.ViewModels.Addresses;
using MyOnlineShop.Common.ViewModels.Products;
using MyOnlineShop.Common.ViewModels.ShoppingCarts;

namespace MyOnlineShop.WebMVC.MappingProfiles
{
    public class ShoppingCartProfile : Profile
    {
        public ShoppingCartProfile()
        {
            this.CreateMap<AddressViewModel, OrderAddressViewModel>()
                .ReverseMap();

            this.CreateMap<ProductDetailsViewModel, CartItemViewModel>()
                .ForMember(dest => dest.ProductId, options => options.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductName, options => options.MapFrom(src => src.Name))
                .ForMember(dest => dest.ProductPrice, options => options.MapFrom(src => src.Price))
                .ForMember(dest => dest.ProductWeight, options => options.MapFrom(src => src.Weight))
                .ForMember(dest => dest.ProductImageUrl, options => options.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.ProductDescription, options => options.MapFrom(src => src.Description))
                .ForMember(dest => dest.Quantity, options => options.MapFrom(src => 1))
                .ForMember(dest => dest.Total, options => options.Ignore());
        }
    }
}
