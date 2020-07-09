using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static MyOnlineShop.WebMVC.Constants.AdminConstants;

namespace MyOnlineShop.WebMVC.Areas.Admin.Controllers
{
    [Authorize(Roles = AdministratorRole)]
    [Area(AdminArea)]
    public class OrdersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
