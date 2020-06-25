using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static MyOnlineShop.Constants.AdminConstants;

namespace MyOnlineShop.Areas.Admin.Controllers
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
