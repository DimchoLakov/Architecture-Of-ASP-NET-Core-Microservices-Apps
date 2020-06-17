using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyOnlineShop.Areas.Admin.Constants;
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

        public async Task<IActionResult> Details(int id)
        {
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

            return View(productDetailsViewModel);
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

            return View(createProductViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

                var categoryExists = await this.dbContext
                    .Categories
                    .AnyAsync(x => x.Id == createProductViewModel.CategoryId);

                if (!categoryExists)
                {
                    throw new Exception(CategoryConstants.DoesNotExistMessage);
                }

                var product = this.mapper.Map<CreateProductViewModel, Product>(createProductViewModel);
                var productCategory = new ProductCategory
                {
                    CategoryId = createProductViewModel.CategoryId
                };
                product
                    .ProductCategories
                    .Add(productCategory);

                await this.dbContext
                    .Products
                    .AddAsync(product);

                return RedirectToAction(nameof(Index));

            }

            return View(createProductViewModel);
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
