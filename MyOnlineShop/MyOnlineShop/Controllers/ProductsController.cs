using Microsoft.AspNetCore.Mvc;

namespace MyOnlineShop.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
