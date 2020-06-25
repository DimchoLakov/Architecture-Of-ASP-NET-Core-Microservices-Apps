using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyOnlineShop.Data;
using MyOnlineShop.ViewModels.Orders;
using System;
using System.Linq;
using System.Threading.Tasks;

using static MyOnlineShop.Constants.DateTimeConstants;
using static MyOnlineShop.Constants.OrderConstants;

namespace MyOnlineShop.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public OrdersController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var orderIndexViewModels = await this.dbContext
                .Orders
                .Select(x => new OrderIndexViewModel
                {
                    Id = x.Id,
                    Date = x.Date.ToString(DateFullMonthYearFormat),
                    Orders = string.Join(", ", x
                                               .OrderItems
                                               .Select(oi => oi.Product.Name + " x " + oi.Quantity)
                                               .ToList())
                })
                .ToListAsync();

            return View(orderIndexViewModels);
        }

        public async Task<IActionResult> Details(int id)
        {
            var orderExists = await this.dbContext
                .Orders
                .AnyAsync(x => x.Id == id);

            if (!orderExists)
            {
                throw new ArgumentException(OrderDoesNotExistMessage);
            }

            var orderDetailsViewModel = await this.dbContext
                .Orders
                .Where(x => x.Id == id)
                .Select(x => new OrderDetailsViewModel
                {
                    Date = x.Date.ToString(DateFullMonthYearHoursMinutesFormat),
                    DeliveryCost = x.DeliveryCost,
                    OrderItemDetailsViewModels = x
                                                  .OrderItems
                                                  .Select(oi => new OrderItemDetailsViewModel 
                                                  {
                                                      PrimaryImageId = oi
                                                                         .Product
                                                                         .Images
                                                                         .Where(i => i.IsPrimary)
                                                                         .Select(i => i.Id)
                                                                         .FirstOrDefault(),
                                                      PrimaryImageName = oi
                                                                         .Product
                                                                         .Images
                                                                         .Where(i => i.IsPrimary)
                                                                         .Select(i => i.Name)
                                                                         .FirstOrDefault(),
                                                      ProductDescription = oi
                                                                             .Product
                                                                             .Description,
                                                      ProductName = oi
                                                                      .Product
                                                                      .Name,
                                                      ProductPrice = oi
                                                                       .Product
                                                                       .Price,
                                                      Quantity = oi.Quantity
                                                  })
                                                  .ToList()
                })
                .FirstOrDefaultAsync();

            return this.View(orderDetailsViewModel);
        }
    }
}
