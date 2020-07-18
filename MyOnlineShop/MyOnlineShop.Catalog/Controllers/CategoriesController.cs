using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class CategoriesController : ApiController
    {
        private readonly CatalogDbContext dbContext;

        public CategoriesController(CatalogDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryIndexViewModel>>> Index()
        {
            var categoryIndexViewModels = await this.dbContext
                .Categories
                .Take(CategoryConstants.MaxTake)
                .Select(x => new CategoryIndexViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return this.Ok(categoryIndexViewModels);
        }

        [HttpPost]
        [Authorize(Roles = AdminConstants.AdministratorRole)]
        public async Task<ActionResult> Add(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                var categoryExists = await this.dbContext
                    .Categories
                    .AnyAsync(x => x.Name.ToLower().Contains(name));

                if (categoryExists)
                {
                    return BadRequest(CategoryConstants.CategoryAlreadyExistsMessage);
                }

                var newCategory = new Category
                {
                    Name = name,
                    IsActive = true
                };

                await this.dbContext
                    .Categories
                    .AddAsync(newCategory);

                await this.dbContext
                    .SaveChangesAsync();

                return this.Ok();
            }

            return this.BadRequest(CategoryConstants.InvalidCategoryNameMessage);
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
                return this.BadRequest(CategoryConstants.CategoryDoesNotExistMessage);
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
                                                  ImageUrl = cp.Product.ImageUrl
                                              })
                })
                .FirstOrDefaultAsync();

            return this.Ok(categoryDetailsViewModel);
        }

        [HttpPut]
        [Route(Id)]
        [Authorize(Roles = AdminConstants.AdministratorRole)]
        public async Task<ActionResult> StatusChange(int id)
        {
            var category = await this.dbContext
                .Categories
                .FindAsync(id);

            if (category == null)
            {
                return this.BadRequest(CategoryConstants.CategoryDoesNotExistMessage);
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
