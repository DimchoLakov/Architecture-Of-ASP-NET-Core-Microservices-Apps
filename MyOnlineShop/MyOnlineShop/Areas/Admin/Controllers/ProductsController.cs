using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyOnlineShop.Areas.Admin.Constants;
using MyOnlineShop.Areas.Admin.ViewModels.Products;
using MyOnlineShop.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MyOnlineShop.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public ProductsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var productIndexViewModels = await this.dbContext
                .Products
                .Take(ProductConstants.MaxTakeCount)
                .Select(x => new ProductIndexViewModel()
                {
                    Id = x.Id,
                    Description = x.Description,
                    Name = x.Name,
                    Price = x.Price,
                    ImageViewModel = new ProductImageViewModel()
                    {
                        Id = x
                        .Images
                        .Where(i => i.IsPrimary)
                        .Select(i => i.Id)
                        .FirstOrDefault(),
                        Name = x
                        .Images
                        .Where(i => i.IsPrimary)
                        .Select(i => i.Name)
                        .FirstOrDefault(),
                    }
                })
                .ToListAsync();

            return View(productIndexViewModels);
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateProductViewModel createProductViewModel)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditProductViewModel editProductViewModel)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }
    }
}
