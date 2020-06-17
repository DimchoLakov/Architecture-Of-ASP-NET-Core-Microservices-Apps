using AutoMapper;
using MyOnlineShop.Areas.Admin.ViewModels.Products;
using MyOnlineShop.Models.Products;

namespace MyOnlineShop.MappingProfiles.Admin
{
    public class ProductsProfile : Profile
    {
        public ProductsProfile()
        {
            this.CreateMap<Product, ProductIndexViewModel>();
            this.CreateMap<Image, ProductImageViewModel>();
        }
    }
}
