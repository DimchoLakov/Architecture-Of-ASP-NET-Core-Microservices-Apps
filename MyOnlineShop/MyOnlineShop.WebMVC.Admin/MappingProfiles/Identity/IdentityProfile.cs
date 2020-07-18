using AutoMapper;
using MyOnlineShop.WebMVC.Admin.ViewModels.Identity;

namespace MyOnlineShop.WebMVC.Admin.MappingProfiles.Identity
{
    public class IdentityProfile : Profile
    {
        public IdentityProfile()
        {
            this.CreateMap<LoginViewModel, LoginCustomerInputModel>();
            this.CreateMap<RegisterViewModel, RegisterCustomerInputModel>();
        }
    }
}
