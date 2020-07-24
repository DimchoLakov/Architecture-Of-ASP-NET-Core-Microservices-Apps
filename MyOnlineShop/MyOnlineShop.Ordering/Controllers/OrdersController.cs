using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyOnlineShop.Common.Constants;
using MyOnlineShop.Common.Controllers;
using MyOnlineShop.Common.Data.Models;
using MyOnlineShop.Common.Messages.Ordering;
using MyOnlineShop.Common.Services;
using MyOnlineShop.Common.ViewModels.Orders;
using MyOnlineShop.Common.ViewModels.ShoppingCarts;
using MyOnlineShop.Ordering.Constants;
using MyOnlineShop.Ordering.Data;
using MyOnlineShop.Ordering.Data.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyOnlineShop.Ordering.Controllers
{
    [Authorize]
    public class OrdersController : ApiController
    {
        private readonly OrderingDbContext orderingDbContext;
        private readonly ICurrentUserService currentUser;
        private readonly IMapper mapper;
        private readonly IBus publisher;

        public OrdersController(
            OrderingDbContext dbContext, 
            ICurrentUserService currentUser,
            IMapper mapper,
            IBus publisher)
        {
            this.orderingDbContext = dbContext;
            this.currentUser = currentUser;
            this.mapper = mapper;
            this.publisher = publisher;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<OrderIndexViewModel>>> UserOrders([FromQuery] string userId)
        {
            var orderIndexViewModels = await this.orderingDbContext
                .Orders
                .Where(x => x.UserId == userId)
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

            if (!orderIndexViewModels.Any())
            {
                return this.Ok(new List<OrderIndexViewModel>());
            }

            return this.Ok(orderIndexViewModels);
        }

        [HttpGet(Id)]
        public async Task<ActionResult<OrderDetailsViewModel>> Details(int id)
        {
            var orderExists = await this.orderingDbContext
                .Orders
                .AnyAsync(x => x.Id == id &&
                               x.UserId == this.currentUser.UserId);

            if (!orderExists)
            {
                return this.BadRequest(OrderConstants.OrderDoesNotExistMessage);
            }

            var orderDetailsViewModel = await this.orderingDbContext
                .Orders
                .Where(x => x.Id == id &&
                            x.UserId == this.currentUser.UserId)
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
                                                      Quantity = oi.Quantity,
                                                      PrimaryImageUrl = oi.ProductImageUrl,
                                                      ProductDescription = oi.ProductDescription
                                                  })
                                                  .ToList()
                })
                .FirstOrDefaultAsync();

            return this.Ok(orderDetailsViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> PlaceOrder([FromQuery] string userId, [FromQuery] int addressId, [FromBody] IEnumerable<CartItemViewModel> cartItemViewModels)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(OrderingConstants.InvalidOrderMessage);
            }

            var order = new Order
            {
                Date = DateTime.Now,
                DeliveryAddressId = addressId,
                DeliveryCost = new Random().Next(1, 9),
                OrderItems = this.mapper.Map<IEnumerable<CartItemViewModel>, IEnumerable<OrderItem>>(cartItemViewModels).ToList(),
                UserId = userId
            };

            var totalOrdersCount = await this.orderingDbContext
                .Orders
                .CountAsync();

            var messageData = new OrderPlacedMessage
            {
                UserId = userId,
                Total = totalOrdersCount + 1
            };

            var message = new Message(messageData);

            await this.orderingDbContext
                .Messages
                .AddAsync(message);

            await this.orderingDbContext
                .Orders
                .AddAsync(order);

            await this.orderingDbContext
                .SaveChangesAsync();

            var msg = await this.orderingDbContext
                .Messages
                .FindAsync(message.Id);

            await this.publisher.Publish(messageData);

            msg.MarkAsPublished();

            await this.orderingDbContext
                .SaveChangesAsync();

            return this.Ok();
        }
    }
}
