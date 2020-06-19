using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyOnlineShop.Areas.Admin.Constants;
using MyOnlineShop.Areas.Admin.ViewModels.Pagination;
using MyOnlineShop.Areas.Admin.ViewModels.Products;
using MyOnlineShop.Data;
using MyOnlineShop.Data.Models.Galleries;
using MyOnlineShop.Data.Models.Products;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static MyOnlineShop.Areas.Admin.Constants.ProductConstants;

namespace MyOnlineShop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public ProductsController(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index(int? currentPage = 1, string search = null)
        {
            int count = await this.dbContext
                .Products
                .CountAsync();

            int size = MaxTakeCount;
            int totalPages = (int)Math.Ceiling(decimal.Divide(count, size));

            if (currentPage < 1)
            {
                currentPage = 1;
            }
            if (currentPage > totalPages)
            {
                currentPage = totalPages;
            }

            int skip = (int)(currentPage - 1) * size;
            if (skip < 0)
            {
                skip = 0;
            }
            int take = size;

            var query = this.dbContext
                .Products
                .Where(x => !x.IsArchived);
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query
                    .Where(x => x.Name.ToLower().Contains(search.ToLower()));
            }

            var productPaginationViewModel = new ProductPaginationViewModel
            {
                Search = search,
                PaginationViewModel = new PaginationViewModel
                {
                    CurrentPage = currentPage.Value,
                    TotalPages = totalPages
                },
                ProductIndexViewModels = await query
                .Skip(skip)
                .Take(take)
                .Take(MaxTakeCount)
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
                .ToListAsync()
            };

            return this.View(productPaginationViewModel);
        }

        public async Task<IActionResult> Details(int id, int? fromPage = 1)
        {
            var productExists = await this.dbContext
                .Products
                .AnyAsync(x => x.Id == id);

            if (!productExists)
            {
                return this.BadRequest(ProductConstants.ProductDoesNotExistMessage);
            }

            var productDetailsViewModel = await this.dbContext
                .Products
                .Where(x => x.Id == id)
                .Select(x => new ProductDetailsViewModel
                {
                    Id = x.Id,
                    Description = x.Description,
                    Name = x.Name,
                    Price = x.Price,
                    StockAvailable = x.StockAvailable,
                    Weight = x.Weight,
                    DateAdded = x.DateAdded,
                    LastUpdated = x.LastUpdated,
                    IsArchived = x.IsArchived,
                    FromPageNumber = fromPage.Value,
                    PrimaryImageViewModel = new ProductImageViewModel
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
                              .FirstOrDefault()
                    },
                    ImageViewModels = x
                        .Images
                        .Select(i => new ProductImageViewModel
                        {
                            Id = i.Id,
                            Name = i.Name
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            return this.View(productDetailsViewModel);
        }

        public async Task<IActionResult> Create()
        {
            var createProductViewModel = new CreateProductViewModel
            {
                Categories = await this.dbContext
                    .Categories
                    .Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    })
                    .ToListAsync()
            };

            return this.View(createProductViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductViewModel createProductViewModel)
        {
            if (this.ModelState.IsValid)
            {
                var productNameExists = await this.dbContext
                    .Products
                    .AnyAsync(x => x.Name.ToLower() == createProductViewModel.Name.ToLower());

                if (productNameExists)
                {
                    throw new Exception(string.Format(ProductConstants.ProductAlreadyExists, createProductViewModel.Name));
                }

                var product = this.mapper.Map<CreateProductViewModel, Product>(createProductViewModel);

                if (createProductViewModel.CategoryId.HasValue)
                {
                    var categoryExists = await this.dbContext
                        .Categories
                        .AnyAsync(x => x.Id == createProductViewModel.CategoryId);

                    if (categoryExists)
                    {
                        var productCategory = new ProductCategory
                        {
                            CategoryId = createProductViewModel.CategoryId.Value
                        };
                        product
                            .ProductCategories
                            .Add(productCategory);
                    }
                }

                if (createProductViewModel.Files.Any())
                {
                    var imageTypes = new string[]
                    {
                        ".tif", ".tiff", ".bmp", ".jpg", ".jpeg", ".gif", ".png", ".eps", ".raw", ".cr2", ".nef", ".orf", ".sr2"
                    };

                    var count = 0;
                    foreach (IFormFile file in createProductViewModel.Files)
                    {
                        count++;
                        string imageExtension = Path.GetExtension(file.FileName);
                        if (!imageTypes.Contains(imageExtension))
                        {
                            return this.BadRequest(string.Format(ImageConstants.ImageTypeNotAllowedMessage, imageExtension));
                        }

                        var image = new Image
                        {
                            Name = file.FileName,
                            IsPrimary = count == 1,
                            MimeType = imageExtension
                        };

                        using var memoryStream = new MemoryStream();
                        file.CopyTo(memoryStream);
                        byte[] fileBytes = memoryStream.ToArray();
                        image.Content = fileBytes;

                        product.Images.Add(image);
                    }
                }

                await this.dbContext
                    .Products
                    .AddAsync(product);

                await this.dbContext
                    .SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }

            return this.View(createProductViewModel);
        }

        public async Task<IActionResult> Edit(int id, int? fromPage = 1)
        {
            var productExists = await this.dbContext
               .Products
               .AnyAsync(x => x.Id == id);

            if (!productExists)
            {
                return this.BadRequest(ProductConstants.ProductDoesNotExistMessage);
            }

            var editProductViewModel = await this.dbContext
                .Products
                .Where(x => x.Id == id)
                .Select(x => new EditProductViewModel
                {
                    Id = x.Id,
                    Description = x.Description,
                    Name = x.Name,
                    Price = x.Price,
                    StockAvailable = x.StockAvailable,
                    Weight = x.Weight,
                    DateAdded = x.DateAdded,
                    LastUpdated = x.LastUpdated,
                    IsArchived = x.IsArchived,
                    FromPageNumber = fromPage.Value,
                    PrimaryImageViewModel = new ProductImageViewModel
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
                              .FirstOrDefault()
                    },
                    ImageViewModels = x
                        .Images
                        .Select(i => new ProductImageViewModel
                        {
                            Id = i.Id,
                            Name = i.Name
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            return this.View(editProductViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProductViewModel editProductViewModel)
        {
            var productExists = await this.dbContext
               .Products
               .AnyAsync(x => x.Id == editProductViewModel.Id);

            if (!productExists)
            {
                return this.BadRequest(ProductConstants.ProductDoesNotExistMessage);
            }

            if (this.ModelState.IsValid)
            {
                var product = await this.dbContext
                    .Products
                    .Include(x => x.Images)
                    .FirstOrDefaultAsync(x => x.Id == editProductViewModel.Id);

                this.mapper.Map(editProductViewModel, product);

                this.dbContext
                    .Products
                    .Update(product);

                await this.dbContext
                .SaveChangesAsync();

                return this.RedirectToAction(nameof(Index), new { currentPage = editProductViewModel.FromPageNumber });
            }


            return this.View(editProductViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Archive(int id)
        {
            var product = await this.dbContext
                .Products
                .FindAsync(id);

            if (product == null)
            {
                throw new ArgumentException(ProductConstants.ProductDoesNotExistMessage);
            }

            product.IsArchived = true;

            return this.RedirectToAction(nameof(Index));
        }
    }
}
