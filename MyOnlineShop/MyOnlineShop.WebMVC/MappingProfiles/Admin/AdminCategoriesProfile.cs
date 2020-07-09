using AutoMapper;
using MyOnlineShop.WebMVC.Areas.Admin.ViewModels.Categories;
using MyOnlineShop.WebMVC.Data.Models.Categories;

namespace MyOnlineShop.WebMVC.MappingProfiles.Admin
{
    public class AdminCategoriesProfile : Profile
    {
        public AdminCategoriesProfile()
        {
            this.CreateMap<AddCategoryViewModel, Category>();
        }
    }
}
