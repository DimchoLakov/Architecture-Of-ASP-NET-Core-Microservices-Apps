using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyOnlineShop.Common.ViewModels.Categories;
using MyOnlineShop.Common.ViewModels.Products;
using System.Linq;
using System.Threading.Tasks;
using MyOnlineShop.Common.Constants;
using MyOnlineShop.WebMVC.Services.Catalog;
using System;
using System.Collections.Generic;

namespace MyOnlineShop.WebMVC.Areas.Admin.Controllers
{
    [Authorize(Roles = AuthConstants.AdministratorRoleName)]
    [Area(AuthConstants.AdminAreaName)]
    public class CategoriesController : Controller
    {
        private readonly ICatalogService catalogService;
        private readonly IMapper mapper;

        public CategoriesController(
            ICatalogService catalogService,
            IMapper mapper)
        {
            this.catalogService = catalogService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var categoryIndexViewModels = await this.catalogService.GetCategories();

                return View(categoryIndexViewModels);
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return View(new List<CategoryIndexViewModel>());
        }

        //public IActionResult Add()
        //{
        //    var addCategoryViewModel = new AddCategoryViewModel();

        //    return this.View(addCategoryViewModel);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Add(AddCategoryViewModel addCategoryViewModel)
        //{
        //    if (this.ModelState.IsValid)
        //    {
        //        var categoryExists = await this.dbContext
        //            .Categories
        //            .AnyAsync(x => x.Name.ToLower().Contains(addCategoryViewModel.Name.ToLower()));

        //        if (categoryExists)
        //        {
        //            return BadRequest();
        //        }

        //        var newCategory = this.mapper.Map<AddCategoryViewModel, Category>(addCategoryViewModel);

        //        await this.dbContext
        //            .Categories
        //            .AddAsync(newCategory);

        //        await this.dbContext
        //            .SaveChangesAsync();

        //        return this.RedirectToAction(nameof(Index));
        //    }

        //    return this.View(addCategoryViewModel);
        //}

        [HttpPost]
        public async Task<IActionResult> StatusChange(int id)
        {
            try
            {
                await this.catalogService.ChangeCategoryStatus(id);

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return this.RedirectToAction(nameof(Index));
        }

        private void HandleException(Exception ex)
        {
            ViewBag.CatalogInoperativeMsg = $"Catalog Service is inoperative {ex.GetType().Name} - {ex.Message}";
        }
    }
}
