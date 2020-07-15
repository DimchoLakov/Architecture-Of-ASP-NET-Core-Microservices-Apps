using AutoMapper;
using MyOnlineShop.WebMVC.ViewModels.Identity;

namespace MyOnlineShop.WebMVC.MappingProfiles.Identity
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
