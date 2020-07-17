using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyOnlineShop.Common.Services;
using MyOnlineShop.WebMVC.Services.Ordering;
using System;
using System.Threading.Tasks;

namespace MyOnlineShop.WebMVC.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderingService orderingService;
        private readonly ICurrentUserService currentUserService;

        public OrdersController(
            IOrderingService orderingService,
            ICurrentUserService currentUserService)
        {
            this.orderingService = orderingService;
            this.currentUserService = currentUserService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var orderIndexViewModels = await this.orderingService.GetUserOrders(this.currentUserService.UserId);

                return this.View(orderIndexViewModels);
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return this.View();
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var orderDetailsViewModel = await this.orderingService.Details(id);

                return this.View(orderDetailsViewModel);
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return this.View();
        }

        private void HandleException(Exception ex)
        {
            ViewBag.OrderingInoperativeMsg = $"Ordering Service is inoperative {ex.GetType().Name} - {ex.Message}";
        }
    }
}
