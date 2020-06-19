using AutoMapper;
using MyOnlineShop.Areas.Admin.ViewModels.Products;
using MyOnlineShop.Data.Models.Galleries;
using MyOnlineShop.Data.Models.Products;

namespace MyOnlineShop.MappingProfiles.Admin
{
    public class ProductsProfile : Profile
    {
        public ProductsProfile()
        {
            this.CreateMap<Product, ProductIndexViewModel>();
            
            this.CreateMap<Image, ProductImageViewModel>();

            this.CreateMap<CreateProductViewModel, Product>()
                .ForMember(dest => dest.ProductCategories, opts => opts.Ignore());

            this.CreateMap<EditProductViewModel, Product>()
                .ForMember(dest => dest.ProductCategories, opts => opts.Ignore());
        }
    }
}
