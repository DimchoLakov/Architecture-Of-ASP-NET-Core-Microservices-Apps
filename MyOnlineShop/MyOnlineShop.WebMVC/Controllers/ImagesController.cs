using Microsoft.AspNetCore.Mvc;
using MyOnlineShop.WebMVC.Constants;
using MyOnlineShop.WebMVC.Services.Catalog;
using System;
using System.Threading.Tasks;

namespace MyOnlineShop.WebMVC.Controllers
{
    public class ImagesController : Controller
    {
        private readonly ICatalogService catalogService;

        public ImagesController(ICatalogService catalogService)
        {
            this.catalogService = catalogService;
        }

        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var imageViewModel = await this.catalogService.GetImage(id);

                return new FileContentResult(imageViewModel.Content, imageViewModel.ContentType);
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return this.BadRequest(ImageConstants.ImageDoesNotExistMessage);
        }

        private void HandleException(Exception ex)
        {
            ViewBag.CatalogInoperativeMsg = $"Catalog Service is inoperative {ex.GetType().Name} - {ex.Message}";
        }
    }
}
