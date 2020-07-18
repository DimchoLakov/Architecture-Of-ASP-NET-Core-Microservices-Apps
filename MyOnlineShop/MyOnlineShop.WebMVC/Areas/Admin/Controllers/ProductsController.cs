﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyOnlineShop.Common.Constants;
using MyOnlineShop.Common.Services;
using MyOnlineShop.Common.ViewModels.Products;
using MyOnlineShop.WebMVC.Services.Catalog;
using System;
using System.Threading.Tasks;
namespace MyOnlineShop.WebMVC.Areas.Admin.Controllers
{
    [Authorize(Roles = AuthConstants.AdministratorRoleName)]
    [Area(AuthConstants.AdminAreaName)]
    public class ProductsController : Controller
    {
        private readonly ICatalogService catalogService;
        private readonly ICurrentTokenService currentTokenService;
        private readonly IMapper mapper;

        public ProductsController(
            ICatalogService catalogService, 
            IMapper mapper,
            ICurrentTokenService currentTokenService)
        {
            this.catalogService = catalogService;
            this.currentTokenService = currentTokenService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index(int? currentPage = 1, string search = null)
        {
            try
            {
                var productPaginationViewModel = await this.catalogService.GetProductPagination(AuthConstants.AdminAreaName, currentPage, search);

                return this.View(productPaginationViewModel);
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return this.View(new ProductPaginationViewModel());
        }

        public async Task<IActionResult> Details(int id, int? fromPage = 1)
        {
            try
            {
                var productDetailsViewModel = await this.catalogService.GetProductDetails(id, fromPage);

                return this.View(productDetailsViewModel);
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return this.View(new ProductDetailsViewModel());
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                var createProductViewModel = await this.catalogService.GetCreateProduct();

                return this.View(createProductViewModel);
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return this.View(new CreateProductViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductViewModel createProductViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(createProductViewModel);
            }

            try
            {
                await this.catalogService.CreateProduct(createProductViewModel);

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return this.View(createProductViewModel);
        }

        public async Task<IActionResult> Edit(int id, int? fromPage = 1)
        {
            try
            {
                var editProductViewModel = await this.catalogService.GetEditProduct(id, fromPage);

                return this.View(editProductViewModel);
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return this.View(new EditProductViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProductViewModel editProductViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(editProductViewModel);
            }

            try
            {
                await this.catalogService.EditProduct(editProductViewModel);

                return this.RedirectToAction(nameof(Index), new { currentPage = editProductViewModel.FromPageNumber });
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return this.View(editProductViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Archive(int id)
        {
            try
            {
                await this.catalogService.ArchiveProduct(id);

                return this.RedirectToAction(nameof(Edit), new { id = id });
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return this.RedirectToAction(nameof(Edit), new { id = id });
        }

        [HttpPost]
        public async Task<IActionResult> Unarchive(int id)
        {
            try
            {
                await this.catalogService.UnarchiveProduct(id);

                return this.RedirectToAction(nameof(Edit), new { id = id });
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return this.RedirectToAction(nameof(Edit), new { id = id });
        }

        private void HandleException(Exception ex)
        {
            ViewBag.CatalogInoperativeMsg = $"Catalog Service is inoperative {ex.GetType().Name} - {ex.Message}";
        }
    }
}
