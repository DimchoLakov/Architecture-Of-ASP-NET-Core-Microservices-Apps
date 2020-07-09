using AutoMapper;
using MyOnlineShop.WebMVC.Data.Models.Customers;
using MyOnlineShop.WebMVC.ViewModels.Addresses;

namespace MyOnlineShop.WebMVC.MappingProfiles
{
    public class ShoppingCartProfile : Profile
    {
        public ShoppingCartProfile()
        {
            this.CreateMap<OrderAddressViewModel, Address>()
                .ReverseMap();
        }
    }
}
