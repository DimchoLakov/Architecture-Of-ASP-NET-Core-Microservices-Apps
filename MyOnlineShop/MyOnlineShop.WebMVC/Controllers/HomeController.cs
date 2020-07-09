using Microsoft.AspNetCore.Mvc;
using MyOnlineShop.WebMVC.ViewModels.Errors;
using System.Diagnostics;

namespace MyOnlineShop.WebMVC.Controllers
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
