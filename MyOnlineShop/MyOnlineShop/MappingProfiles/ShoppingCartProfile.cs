using AutoMapper;
using MyOnlineShop.Data.Models.Customers;
using MyOnlineShop.ViewModels.Addresses;

namespace MyOnlineShop.MappingProfiles
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
