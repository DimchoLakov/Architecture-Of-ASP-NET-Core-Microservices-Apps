using AutoMapper;
using MyOnlineShop.WebMVC.Areas.Admin.ViewModels.Products;
using MyOnlineShop.WebMVC.Data.Models.Galleries;
using MyOnlineShop.WebMVC.Data.Models.Products;
using System;

namespace MyOnlineShop.WebMVC.MappingProfiles.Admin
{
    public class AdminProductsProfile : Profile
    {
        public AdminProductsProfile()
        {
            this.CreateMap<Product, ProductIndexViewModel>();
            
            this.CreateMap<Image, ProductImageViewModel>();

            this.CreateMap<CreateProductViewModel, Product>()
                .ForMember(dest => dest.ProductCategories, opts => opts.Ignore())
                .ForMember(dest => dest.DateAdded, opts => opts.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.LastUpdated, opts => opts.MapFrom(src => DateTime.UtcNow));

            this.CreateMap<EditProductViewModel, Product>()
                .ForMember(dest => dest.ProductCategories, opts => opts.Ignore())
                .ForMember(dest => dest.DateAdded, opts => opts.Ignore())
                .ForMember(dest => dest.LastUpdated, opts => opts.MapFrom(src => DateTime.UtcNow));
        }
    }
}
