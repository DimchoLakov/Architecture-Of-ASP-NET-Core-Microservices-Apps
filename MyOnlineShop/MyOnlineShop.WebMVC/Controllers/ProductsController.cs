using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyOnlineShop.WebMVC.Services.Catalog;
using System;
using System.Threading.Tasks;

namespace MyOnlineShop.WebMVC.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ICatalogService catalogService;

        public ProductsController(ICatalogService catalogService)
        {
            this.catalogService = catalogService;
        }

        public async Task<IActionResult> Index(int? currentPage = 1, string search = null)
        {
            try
            {
                var productPaginationViewModel = await this.catalogService.GetProductPagination(string.Empty, currentPage, search);

                return this.View(productPaginationViewModel);
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return this.BadRequest();
        }

        public async Task<IActionResult> Details(int id, int? fromPage = 1)
        {
            try
            {
                var productDetailsViewModel = await this.catalogService.GetProductDetails(id, fromPage);

                return this.View(productDetailsViewModel);
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return this.BadRequest();
        }

        private void HandleException(Exception ex)
        {
            ViewBag.CatalogInoperativeMsg = $"Catalog Service is inoperative {ex.GetType().Name} - {ex.Message}";
        }
    }
}
