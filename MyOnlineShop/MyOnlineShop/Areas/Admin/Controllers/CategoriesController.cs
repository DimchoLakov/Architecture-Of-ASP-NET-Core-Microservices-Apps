using Microsoft.AspNetCore.Mvc;

namespace MyOnlineShop.Areas.Admin.Controllers
{
    public class CategoriesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
