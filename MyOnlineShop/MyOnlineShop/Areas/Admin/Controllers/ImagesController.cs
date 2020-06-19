using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyOnlineShop.Areas.Admin.Constants;
using MyOnlineShop.Areas.Admin.ViewModels.Images;
using MyOnlineShop.Data;
using MyOnlineShop.Models.Products;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyOnlineShop.Areas.Admin.Controllers
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
                return this.BadRequest(ImageConstants.ImageDoesNotExistMessage);
            }

            return new FileContentResult(image.Content, "image/jpeg");
        }

        [HttpPost]
        public async Task<IActionResult> AddToProduct(AddImageViewModel addImageViewModel, int productId)
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

                    var image = new Image();
                    image.Name = addImageViewModel.Name;
                    image.IsPrimary = addImageViewModel.IsPrimary;
                    image.MimeType = imageExtension;
                    image.ProductId = productId;

                    using var memoryStream = new MemoryStream();
                    addImageViewModel.File.CopyTo(memoryStream);
                    byte[] fileBytes = memoryStream.ToArray();
                    image.Content = fileBytes;

                    await this.dbContext
                        .Images
                        .AddAsync(image);

                    await this.dbContext
                        .SaveChangesAsync();

                    return this.Redirect("/Admin/Products/Details/?id=" + productId);
                }
            }

            return this.BadRequest();
        }
    }
}
