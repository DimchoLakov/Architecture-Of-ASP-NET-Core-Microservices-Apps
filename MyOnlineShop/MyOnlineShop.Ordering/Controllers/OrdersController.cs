using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyOnlineShop.Common.Constants;
using MyOnlineShop.Common.Controllers;
using MyOnlineShop.Common.Services;
using MyOnlineShop.Common.ViewModels.Orders;
using MyOnlineShop.Ordering.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyOnlineShop.Ordering.Controllers
{
    public class OrdersController : ApiController
    {
        private readonly OrderingDbContext dbContext;
        private readonly ICurrentUserService currentUser;

        public OrdersController(OrderingDbContext dbContext, ICurrentUserService currentUser)
        {
            this.dbContext = dbContext;
            this.currentUser = currentUser;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ICollection<OrderIndexViewModel>>> UserOrders()
        {
            if (string.IsNullOrWhiteSpace(this.currentUser.UserId))
            {
                return this.BadRequest();
            }

            var orderIndexViewModels = await this.dbContext
                .Orders
                .Where(x => x.UserId == this.currentUser.UserId)
                .Select(x => new OrderIndexViewModel
                {
                    Id = x.Id,
                    Date = x.Date.ToString(DateTimeConstants.DateFullMonthYearFormat),
                    Orders = string.Join(", ", x
                                               .OrderItems
                                               .Select(oi => oi.ProductName + " x " + oi.Quantity)
                                               .ToList())
                })
                .ToListAsync();

            return this.Ok(orderIndexViewModels);
        }

        [Authorize]
        [HttpGet(Id)]
        public async Task<ActionResult> Details(int id)
        {
            var orderExists = await this.dbContext
                .Orders
                .AnyAsync(x => x.Id == id);

            if (!orderExists)
            {
                return this.BadRequest(OrderConstants.OrderDoesNotExistMessage);
            }

            var orderDetailsViewModel = await this.dbContext
                .Orders
                .Where(x => x.Id == id)
                .Select(x => new OrderDetailsViewModel
                {
                    Date = x.Date.ToString(DateTimeConstants.DateFullMonthYearHoursMinutesFormat),
                    DeliveryCost = x.DeliveryCost,
                    OrderItemDetailsViewModels = x
                                                  .OrderItems
                                                  .Select(oi => new OrderItemDetailsViewModel
                                                  {
                                                      ProductName = oi.ProductName,
                                                      ProductPrice = oi.ProductPrice,
                                                      Quantity = oi.Quantity
                                                  })
                                                  .ToList()
                })
                .FirstOrDefaultAsync();

            return this.Ok(orderDetailsViewModel);
        }
    }
}
