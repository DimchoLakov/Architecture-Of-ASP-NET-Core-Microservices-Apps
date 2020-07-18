using Microsoft.AspNetCore.Mvc.Filters;
using MyOnlineShop.Catalog.Data.Models.Customers;
using MyOnlineShop.Common.Services;
using MyOnlineShop.Ordering.Data;
using System.Linq;

namespace MyOnlineShop.Catalog.Filters
{
    public class AddCustomerActionFilter : IActionFilter
    {
        private readonly CatalogDbContext catalogDbContext;
        private readonly ICurrentUserService currentUserService;

        public AddCustomerActionFilter(
            CatalogDbContext catalogDbContext,
            ICurrentUserService currentUserService)
        {
            this.catalogDbContext = catalogDbContext;
            this.currentUserService = currentUserService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!string.IsNullOrWhiteSpace(currentUserService.UserId))
            {
                var customer = catalogDbContext
                    .Customers
                    .FirstOrDefault(x => x.UserId == currentUserService.UserId);

                if (customer == null)
                {
                    var newCustomer = new Customer
                    {
                        UserId = currentUserService.UserId
                    };

                    catalogDbContext
                        .Customers
                        .Add(newCustomer);

                    catalogDbContext
                        .SaveChanges();
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
