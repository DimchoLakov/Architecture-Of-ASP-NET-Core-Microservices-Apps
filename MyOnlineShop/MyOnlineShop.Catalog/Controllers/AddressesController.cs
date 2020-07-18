using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyOnlineShop.Catalog.Constants;
using MyOnlineShop.Catalog.Data.Models.Customers;
using MyOnlineShop.Catalog.Filters;
using MyOnlineShop.Common.Controllers;
using MyOnlineShop.Common.ViewModels.Addresses;
using MyOnlineShop.Common.ViewModels.ShoppingCarts;
using MyOnlineShop.Ordering.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MyOnlineShop.Catalog.Controllers
{
    [Authorize]
    public class AddressesController : ApiController
    {
        private readonly CatalogDbContext catalogDbContext;
        private readonly IMapper mapper;

        public AddressesController(
            CatalogDbContext catalogDbContext,
            IMapper mapper)
        {
            this.catalogDbContext = catalogDbContext;
            this.mapper = mapper;
        }

        [HttpGet("{userId}")]
        [ServiceFilter(typeof(AddCustomerActionFilter))]
        public async Task<ActionResult<AddressViewModel>> GetAddress(string userId)
        {
            var addressViewModel = await this.catalogDbContext
                .Addresses
                .Include(x => x.Customer)
                .Where(x => x.Customer.UserId == userId &&
                            x.IsDeliveryAddress)
                .Select(x => new AddressViewModel
                {
                    Id = x.Id,
                    AddressLine = x.AddressLine,
                    Country = x.Country,
                    CustomerId = x.CustomerId,
                    IsDeliveryAddress = x.IsDeliveryAddress,
                    PostCode = x.PostCode,
                    Region = x.Region,
                    Town = x.Town,
                    IsAddressAvailable = true
                })
                .FirstOrDefaultAsync();

            if (addressViewModel != null)
            {
                return this.Ok(addressViewModel);
            }

            return this.Ok(new AddressViewModel());
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateAddress([FromQuery] string userId, OrderAddressViewModel orderAddressViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(AddressConstants.InvalidAddressMessage);
            }

            var address = this.mapper.Map<OrderAddressViewModel, Address>(orderAddressViewModel);

            var anyAddresses = await this.catalogDbContext
                .Addresses
                .Include(x => x.Customer)
                .AnyAsync(x => x.Customer.UserId == userId);

            if (!anyAddresses)
            {
                address.IsDeliveryAddress = true;
            }

            var customer = await this.catalogDbContext
                .Customers
                .Include(x => x.Addresses)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (customer == null)
            {
                return this.BadRequest(AddressConstants.CustomerDoesNotExistMessage);
            }

            customer
                .Addresses
                .Add(address);

            this.catalogDbContext
                .Customers
                .Update(customer);

            return this.Ok(await this.catalogDbContext
                .SaveChangesAsync());
        }
    }
}
