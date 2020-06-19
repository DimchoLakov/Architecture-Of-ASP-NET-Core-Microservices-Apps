using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyOnlineShop.Areas.Admin.ViewModels.Categories;
using MyOnlineShop.Areas.Admin.ViewModels.Products;
using MyOnlineShop.Data;
using MyOnlineShop.Data.Models.Categories;
using System.Linq;
using System.Threading.Tasks;

namespace MyOnlineShop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public CategoriesController(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var categoryIndexViewModels = await this.dbContext
                .Categories
                .Take(10)
                .Select(x => new CategoryIndexViewModel
                {
                    Name = x.Name,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return View(categoryIndexViewModels);
        }

        public IActionResult Add()
        {
            var addCategoryViewModel = new AddCategoryViewModel();

            return this.View(addCategoryViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCategoryViewModel addCategoryViewModel)
        {
            if (this.ModelState.IsValid)
            {
                var categoryExists = await this.dbContext
                    .Categories
                    .AnyAsync(x => x.Name.ToLower().Contains(addCategoryViewModel.Name.ToLower()));

                if (categoryExists)
                {
                    return BadRequest();
                }

                var newCategory = this.mapper.Map<AddCategoryViewModel, Category>(addCategoryViewModel);

                await this.dbContext
                    .Categories
                    .AddAsync(newCategory);

                await this.dbContext
                    .SaveChangesAsync();

                return this.RedirectToAction(nameof(Index));
            }

            return this.View(addCategoryViewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var categoryExists = await this.dbContext
                .Categories
                .AnyAsync(x => x.Id == id);

            if (!categoryExists)
            {
                return this.BadRequest();
            }

            var categoryDetailsViewModel = await this.dbContext
                .Categories
                .Where(x => x.Id == id)
                .Select(x => new CategoryDetailsViewModel
                {
                    Name = x.Name,
                    IsActive = x.IsActive,
                    ProductIndexViewModels = x
                                              .CategoryProducts
                                              .Select(cp => new ProductIndexViewModel
                                              {
                                                  Id = cp.Product.Id,
                                                  Name = cp.Product.Name,
                                                  Description = cp.Product.Description,
                                                  ImageViewModel = new ProductImageViewModel
                                                  {
                                                      Id = cp
                                                            .Product
                                                            .Images
                                                            .Where(i => i.IsPrimary)
                                                            .Select(i => i.Id)
                                                            .FirstOrDefault(),
                                                      Name = cp
                                                              .Product
                                                              .Images
                                                              .Where(i => i.IsPrimary)
                                                              .Select(i => i.Name)
                                                              .FirstOrDefault(),
                                                  }
                                              })
                })
                .FirstOrDefaultAsync();

            return this.View(categoryDetailsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> StatusChange(int id)
        {
            var category = await this.dbContext
                .Categories
                .FindAsync(id);

            category.IsActive = !category.IsActive;

            this.dbContext
                .Categories
                .Update(category);

            var rowsAffected = await this.dbContext
                .SaveChangesAsync();

            return this.Json(new
            {
                updated = rowsAffected == 1
            });
        }
    }
}
