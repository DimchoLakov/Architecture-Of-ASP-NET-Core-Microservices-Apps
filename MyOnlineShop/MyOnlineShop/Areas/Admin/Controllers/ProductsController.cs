using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyOnlineShop.Areas.Admin.Constants;
using MyOnlineShop.Areas.Admin.ViewModels.Common;
using MyOnlineShop.Areas.Admin.ViewModels.Products;
using MyOnlineShop.Data;
using MyOnlineShop.Models.Products;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyOnlineShop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public ProductsController(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var productIndexViewModels = await this.dbContext
                .Products
                .Where(x => !x.IsArchived)
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

            return this.View(productIndexViewModels);
        }

        public async Task<IActionResult> Details(int id)
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
                    Description = x.Description,
                    Name = x.Name,
                    Price = x.Price,
                    StockAvailable = x.StockAvailable,
                    Weight = x.Weight,
                    ImageViewModels = x
                        .Images
                        .Select(i => new ProductImageViewModel
                        {
                            Id = i.Id,
                            Name = i.Name
                        })
                        .ToList()
                })
                .ToListAsync();

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

                await this.dbContext
                    .Products
                    .AddAsync(product);

                return RedirectToAction(nameof(Index));

            }

            return this.View(createProductViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var productExists = await this.dbContext
               .Products
               .AnyAsync(x => x.Id == id);

            if (!productExists)
            {
                return this.BadRequest(ProductConstants.ProductDoesNotExistMessage);
            }

            var editProductViewModel = this.dbContext
                .Products
                .Where(x => x.Id == id)
                .Select(x => new EditProductViewModel
                {
                    Id = x.Id,
                    Description = x.Description,
                    Name = x.Name,
                    Price = x.Price,
                    Weight = x.Weight,
                    StockAvailable = x.StockAvailable,
                    ImageViewModels = x
                            .Images
                            .Select(i => new ProductImageViewModel
                            {
                                Id = i.Id,
                                Name = i.Name
                            })
                            .ToList()
                })
                .ToListAsync();

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

                return this.RedirectToAction(nameof(Index));
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
