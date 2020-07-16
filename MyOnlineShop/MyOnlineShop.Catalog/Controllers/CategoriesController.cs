using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyOnlineShop.Catalog.Constants;
using MyOnlineShop.Catalog.Data.Models.Categories;
using MyOnlineShop.Common.Controllers;
using MyOnlineShop.Common.ViewModels.Categories;
using MyOnlineShop.Common.ViewModels.Products;
using MyOnlineShop.Ordering.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyOnlineShop.Catalog.Controllers
{
    public class CategoriesController : ApiController
    {
        private readonly CatalogDbContext dbContext;
        private readonly IMapper mapper;

        public CategoriesController(
            CatalogDbContext dbContext, 
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<CategoryIndexViewModel>>> Index()
        {
            var categoryIndexViewModels = await this.dbContext
                .Categories
                .Take(CategoryConstants.MaxTake)
                .Select(x => new CategoryIndexViewModel
                {
                    Name = x.Name,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return this.Ok(categoryIndexViewModels);
        }

        [HttpPost]
        public async Task<ActionResult> Add(AddCategoryViewModel addCategoryViewModel)
        {
            if (this.ModelState.IsValid)
            {
                var categoryExists = await this.dbContext
                    .Categories
                    .AnyAsync(x => x.Name.ToLower().Contains(addCategoryViewModel.Name.ToLower()));

                if (categoryExists)
                {
                    return BadRequest(CategoryConstants.DoesAlreadyExistMessage);
                }

                var newCategory = this.mapper.Map<AddCategoryViewModel, Category>(addCategoryViewModel);

                await this.dbContext
                    .Categories
                    .AddAsync(newCategory);

                await this.dbContext
                    .SaveChangesAsync();

                return this.Ok();
            }

            var errors = this.ModelState
                .SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage)
                .ToList();

            return this.BadRequest(errors);
        }

        [HttpGet]
        [Route(Id)]
        public async Task<ActionResult<CategoryDetailsViewModel>> Details(int id)
        {
            var categoryExists = await this.dbContext
                .Categories
                .AnyAsync(x => x.Id == id);

            if (!categoryExists)
            {
                return this.BadRequest(CategoryConstants.DoesNotExistMessage);
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

            return this.Ok(categoryDetailsViewModel);
        }

        [HttpPut]
        [Route(Id)]
        public async Task<ActionResult> StatusChange(int id)
        {
            var category = await this.dbContext
                .Categories
                .FindAsync(id);

            if (category == null)
            {
                return this.BadRequest(CategoryConstants.DoesNotExistMessage);
            }

            category.IsActive = !category.IsActive;

            this.dbContext
                .Categories
                .Update(category);

            await this.dbContext
                .SaveChangesAsync();

            return this.Ok();
        }
    }
}
