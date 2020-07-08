using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyOnlineShop.Areas.Admin.ViewModels.Images;
using MyOnlineShop.Data;
using MyOnlineShop.Data.Models.Galleries;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using static MyOnlineShop.Constants.ImageConstants;
using static MyOnlineShop.Constants.ProductConstants;
using static MyOnlineShop.Constants.AdminConstants;

namespace MyOnlineShop.Areas.Admin.Controllers
{
    [Authorize(Roles = AdministratorRole)]
    [Area(AdminArea)]
    public class ImagesController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public ImagesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> AddToProduct(AddImageViewModel addImageViewModel, int productId)
        {
            var productExists = await this.dbContext
               .Products
               .AnyAsync(x => x.Id == productId);

            if (!productExists)
            {
                return this.BadRequest(ProductDoesNotExistMessage);
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
                        return this.BadRequest(string.Format(ImageTypeNotAllowedMessage, imageExtension));
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

                    return this.Redirect("/Admin/Products/Details/?id=" + productId);
                }
            }

            return this.BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var image = await this.dbContext
                .Images
                .FindAsync(id);

            if (image == null)
            {
                return this.BadRequest(ImageDoesNotExistMessage);
            }

            this.dbContext
                .Images
                .Remove(image);

            await this.dbContext
                .SaveChangesAsync();

            return this.Redirect("/Admin/Products/Index");
        }
    }
}
