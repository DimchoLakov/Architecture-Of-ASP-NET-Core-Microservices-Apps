using Microsoft.AspNetCore.Mvc;
using MyOnlineShop.ViewModels.Errors;
using System.Diagnostics;

namespace MyOnlineShop.Controllers
{
    public class HomeController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
