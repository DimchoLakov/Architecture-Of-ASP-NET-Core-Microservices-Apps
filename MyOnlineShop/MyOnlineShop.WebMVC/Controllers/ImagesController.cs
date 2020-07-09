using Microsoft.AspNetCore.Mvc;
using MyOnlineShop.WebMVC.Data;
using System.Threading.Tasks;
using static MyOnlineShop.WebMVC.Constants.ImageConstants;

namespace MyOnlineShop.WebMVC.Controllers
{
    public class ImagesController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public ImagesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IActionResult> Get(int id)
        {
            var image = await this.dbContext
                .Images
                .FindAsync(id);

            if (image == null)
            {
                return this.BadRequest(ImageDoesNotExistMessage);
            }

            return new FileContentResult(image.Content, $"image/{image.MimeType}");
        }

    }
}
