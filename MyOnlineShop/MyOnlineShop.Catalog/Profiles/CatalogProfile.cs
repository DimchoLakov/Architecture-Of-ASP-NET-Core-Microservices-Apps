using AutoMapper;
using MyOnlineShop.Catalog.Data.Models.Categories;
using MyOnlineShop.Catalog.Data.Models.Customers;
using MyOnlineShop.Catalog.Data.Models.Products;
using MyOnlineShop.Common.ViewModels.Addresses;
using MyOnlineShop.Common.ViewModels.Categories;
using MyOnlineShop.Common.ViewModels.Products;
using MyOnlineShop.Common.ViewModels.ShoppingCarts;
using System;

namespace MyOnlineShop.Catalog.Profiles
{
    public class CatalogProfile : Profile
    {
        public CatalogProfile()
        {
            this.CreateMap<Product, ProductIndexViewModel>();

            this.CreateMap<CreateProductViewModel, Product>()
                .ForMember(dest => dest.ProductCategories, opts => opts.Ignore())
                .ForMember(dest => dest.DateAdded, opts => opts.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.LastUpdated, opts => opts.MapFrom(src => DateTime.Now))
                .ReverseMap();

            this.CreateMap<EditProductViewModel, Product>()
                .ForMember(dest => dest.ProductCategories, opts => opts.Ignore())
                .ForMember(dest => dest.DateAdded, opts => opts.Ignore())
                .ForMember(dest => dest.LastUpdated, opts => opts.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.IsArchived, opts => opts.Ignore())
                .ReverseMap();

            this.CreateMap<AddCategoryViewModel, Category>();

            this.CreateMap<Address, AddressViewModel>();

            this.CreateMap<OrderAddressViewModel, Address>()
                .ForMember(dest => dest.IsDeliveryAddress, opts => opts.Ignore())
                .ReverseMap();
        }
    }
}
