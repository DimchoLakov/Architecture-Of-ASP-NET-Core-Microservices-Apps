using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyOnlineShop.Common.Constants;
using MyOnlineShop.Common.ViewModels.Categories;
using MyOnlineShop.WebMVC.Admin.Services.Catalog;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyOnlineShop.WebMVC.Admin.Controllers
{
    [Authorize(Roles = AuthConstants.AdministratorRoleName)]
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
            catch (Refit.ApiException apiEx)
            {
                if (apiEx.HasContent)
                {
                    JsonConvert
                        .DeserializeObject<List<string>>(apiEx.Content)
                        .ForEach(error => this.ModelState.AddModelError(string.Empty, error));
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, ErrorConstants.InternalServerErrorMessage);
                }

                this.HandleException(apiEx);
            }

            return View(new List<CategoryIndexViewModel>());
        }

        [HttpPost]
        public async Task<IActionResult> Add(string name)
        {
            try
            {
                await this.catalogService.AddCategory(name);

                return this.RedirectToAction(nameof(Index));
            }
            catch (Refit.ApiException apiEx)
            {
                if (apiEx.HasContent)
                {
                    JsonConvert
                        .DeserializeObject<List<string>>(apiEx.Content)
                        .ForEach(error => this.ModelState.AddModelError(string.Empty, error));
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, ErrorConstants.InternalServerErrorMessage);
                }

                this.HandleException(apiEx);
            }

            return this.RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> StatusChange(int id)
        {
            try
            {
                await this.catalogService.ChangeCategoryStatus(id);

                return this.RedirectToAction(nameof(Index));
            }
            catch (Refit.ApiException apiEx)
            {
                if (apiEx.HasContent)
                {
                    JsonConvert
                        .DeserializeObject<List<string>>(apiEx.Content)
                        .ForEach(error => this.ModelState.AddModelError(string.Empty, error));
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, ErrorConstants.InternalServerErrorMessage);
                }

                this.HandleException(apiEx);
            }

            return this.RedirectToAction(nameof(Index));
        }

        private void HandleException(Exception ex)
        {
            ViewBag.CatalogInoperativeMsg = $"Catalog Service is inoperative {ex.GetType().Name} - {ex.Message}";
        }
    }
}
