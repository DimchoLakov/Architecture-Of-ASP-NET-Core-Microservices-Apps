using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyOnlineShop.Catalog.Constants;
using MyOnlineShop.Catalog.Data.Models.Galleries;
using MyOnlineShop.Common.Controllers;
using MyOnlineShop.Common.ViewModels.Images;
using MyOnlineShop.Ordering.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyOnlineShop.Catalog.Controllers
{
    public class ImagesController : ApiController
    {
        private readonly CatalogDbContext dbContext;

        public ImagesController(CatalogDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route(Id)]
        public async Task<ActionResult<ImageViewModel>> Get(int id)
        {
            var imageViewModel = await this.dbContext
                .Images
                .Where(x => x.Id == id)
                .Select(x => new ImageViewModel
                {
                    Content = x.Content,
                    ContentType = $"image/{x.MimeType}",
                    Name = x.Name
                })
                .FirstOrDefaultAsync();

            if (imageViewModel == null)
            {
                return this.BadRequest(ImageConstants.ImageDoesNotExistMessage);
            }

            return this.Ok(imageViewModel);
        }

        [HttpDelete]
        [Route(Id)]
        public async Task<ActionResult> Delete(int id)
        {
            var image = await this.dbContext
                .Images
                .FindAsync(id);

            if (image == null)
            {
                return this.BadRequest(ImageConstants.ImageDoesNotExistMessage);
            }

            this.dbContext
                .Images
                .Remove(image);

            await this.dbContext
                .SaveChangesAsync();

            return this.Ok();
        }

        [HttpPost]
        [Route(Id)]
        public async Task<IActionResult> AddToProduct(int productId, [FromBody]AddImageViewModel addImageViewModel)
        {
            var productExists = await this.dbContext
               .Products
               .AnyAsync(x => x.Id == productId);

            if (!productExists)
            {
                return this.BadRequest(ProductConstants.ProductDoesNotExistMessage);
            }

            if (this.ModelState.IsValid)
            {
                if (addImageViewModel.File.Length > 0)
                {
                    var imageTypes = new string[]
                    {
                        ".tif", ".tiff", ".bmp", ".jpg", ".jpeg", ".gif", ".png", ".eps", ".raw", ".cr2", ".nef", ".orf", ".sr2"
                    };

                    string imageExtension = Path.GetExtension(addImageViewModel.File.FileName);
                    if (!imageTypes.Contains(imageExtension))
                    {
                        return this.BadRequest(string.Format(ImageConstants.ImageTypeNotAllowedMessage, imageExtension));
                    }

                    var image = new Image
                    {
                        Name = addImageViewModel.Name,
                        IsPrimary = addImageViewModel.IsPrimary,
                        MimeType = imageExtension,
                        ProductId = productId
                    };

                    using var memoryStream = new MemoryStream();
                    addImageViewModel.File.CopyTo(memoryStream);
                    byte[] fileBytes = memoryStream.ToArray();
                    image.Content = fileBytes;

                    await this.dbContext
                        .Images
                        .AddAsync(image);

                    await this.dbContext
                        .SaveChangesAsync();

                    return this.Ok();
                }
            }

            return this.BadRequest();
        }
    }
}
