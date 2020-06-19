using AutoMapper;
using MyOnlineShop.Areas.Admin.ViewModels.Categories;
using MyOnlineShop.Data.Models.Categories;

namespace MyOnlineShop.MappingProfiles.Admin
{
    public class AdminCategoriesProfile : Profile
    {
        public AdminCategoriesProfile()
        {
            this.CreateMap<AddCategoryViewModel, Category>();
        }
    }
}
