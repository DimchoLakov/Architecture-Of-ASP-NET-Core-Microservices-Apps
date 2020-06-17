using Microsoft.AspNetCore.Mvc;

namespace MyOnlineShop.Areas.Admin.Controllers
{
    public class OrdersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
