using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyOnlineShop.Catalog.Constants;
using MyOnlineShop.Catalog.Data.Models.Products;
using MyOnlineShop.Common.Constants;
using MyOnlineShop.Common.Controllers;
using MyOnlineShop.Common.Data.Models;
using MyOnlineShop.Common.Messages.Catalog;
using MyOnlineShop.Common.Services;
using MyOnlineShop.Common.ViewModels.Pagination;
using MyOnlineShop.Common.ViewModels.Products;
using MyOnlineShop.Ordering.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyOnlineShop.Catalog.Controllers
{
    [Authorize]
    public class ProductsController : ApiController
    {
        private readonly CatalogDbContext dbContext;
        private readonly ICurrentUserService currentUserService;
        private readonly IMapper mapper;
        private readonly IBus publisher;

        public ProductsController(
            CatalogDbContext dbContext,
            ICurrentUserService currentUserService,
            IMapper mapper,
            IBus publisher)
        {
            this.dbContext = dbContext;
            this.currentUserService = currentUserService;
            this.mapper = mapper;
            this.publisher = publisher;
        }

        [HttpGet]
        public async Task<ActionResult<ProductPaginationViewModel>> All(string area, int? currentPage = 1, string search = null)
        {
            int count = 0;

            var query = this.dbContext
                .Products
                .Where(x => !x.IsArchived);

            if (this.currentUserService.IsAdministrator &&
                area == AuthConstants.AdminAreaName)
            {
                query = this.dbContext
                .Products;
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query
                    .Where(x => x.Name.ToLower().Contains(search.ToLower()));

                count = await this.dbContext
                .Products
                .CountAsync(x => x.Name.ToLower().Contains(search.ToLower()));
            }
            else
            {
                count = await this.dbContext
                .Products
                .CountAsync();
            }

            int size = ProductConstants.MaxTakeCount;
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
                .Select(x => new ProductIndexViewModel()
                {
                    Id = x.Id,
                    Description = x.Description,
                    Name = x.Name,
                    Price = x.Price,
                    ImageUrl = x.ImageUrl,
                    IsArchived = x.IsArchived
                })
                .ToListAsync()
            };

            return this.Ok(productPaginationViewModel);
        }

        [HttpGet]
        [Route(Id)]
        public async Task<ActionResult<ProductDetailsViewModel>> Details(int id, int? fromPage = 1)
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
                    FromPageNumber = fromPage.Value,
                    ImageUrl = x.ImageUrl,
                    IsArchived = x.IsArchived,
                    DateAdded = x.DateAdded,
                    LastUpdated = x.LastUpdated
                })
                .FirstOrDefaultAsync();

            return this.Ok(productDetailsViewModel);
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize(Roles = AdminConstants.AdministratorRole)]
        public async Task<ActionResult<CreateProductViewModel>> GetCreate()
        {
            var createProductViewModel = new CreateProductViewModel
            {
                Categories = await this.dbContext
                    .Categories
                    .Where(x => x.IsActive)
                    .Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    })
                    .ToListAsync()
            };

            return this.Ok(createProductViewModel);
        }

        [HttpPost]
        [Authorize(Roles = AdminConstants.AdministratorRole)]
        public async Task<ActionResult> Create([FromBody] CreateProductViewModel createProductViewModel)
        {
            if (this.ModelState.IsValid)
            {
                var productNameExists = await this.dbContext
                    .Products
                    .AnyAsync(x => x.Name.ToLower() == createProductViewModel.Name.ToLower());

                if (productNameExists)
                {
                    return this.BadRequest(string.Format(ProductConstants.ProductAlreadyExists, createProductViewModel.Name));
                }

                var product = this.mapper.Map<CreateProductViewModel, Product>(createProductViewModel);
                product.DateAdded = DateTime.Now;
                product.LastUpdated = DateTime.Now;

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

                var totalProductsCount = await this.dbContext
                    .Products
                    .CountAsync();

                var messageData = new ProductAddedMessage
                {
                    Name = createProductViewModel.Name,
                    Total = totalProductsCount + 1
                };

                var message = new Message(messageData);

                await this.dbContext
                    .Messages
                    .AddAsync(message);

                await this.dbContext
                    .Products
                    .AddAsync(product);

                await this.dbContext
                    .SaveChangesAsync();

                await this.publisher.Publish(messageData);

                var msg = await this.dbContext
                    .Messages
                    .FindAsync(message.Id);

                msg.MarkAsPublished();

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
        [Route("[action]")]
        [Authorize(Roles = AdminConstants.AdministratorRole)]
        public async Task<ActionResult<EditProductViewModel>> GetEdit([FromQuery] int id, [FromQuery] int? fromPage = 1)
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
                    ImageUrl = x.ImageUrl
                })
                .FirstOrDefaultAsync();

            return this.Ok(editProductViewModel);
        }

        [HttpPut]
        [Authorize(Roles = AdminConstants.AdministratorRole)]
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
                    .FirstOrDefaultAsync(x => x.Id == editProductViewModel.Id);

                this.mapper.Map(editProductViewModel, product);

                product.LastUpdated = DateTime.Now;


                var messageData = new ProductUpdatedMessage
                {
                    ProductId = editProductViewModel.Id,
                    Description = editProductViewModel.Description,
                    ImageUrl = editProductViewModel.ImageUrl,
                    Name = editProductViewModel.Name,
                    Price = editProductViewModel.Price,
                    Weight = editProductViewModel.Weight
                };

                var message = new Message(messageData);

                await this.dbContext
                    .Messages
                    .AddAsync(message);

                this.dbContext
                    .Products
                    .Update(product);

                await this.dbContext
                .SaveChangesAsync();

                var msg = await this.dbContext
                    .Messages
                    .FindAsync(message.Id);

                await this.publisher.Publish(messageData);

                msg.MarkAsPublished();

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

        [HttpPost]
        [Route("[action]" + PathSeparator + Id)]
        [Authorize(Roles = AdminConstants.AdministratorRole)]
        public async Task<ActionResult> Archive(int id)
        {
            var product = await this.dbContext
                .Products
                .FindAsync(id);

            if (product == null)
            {
                return this.BadRequest(ProductConstants.ProductDoesNotExistMessage);
            }

            product.IsArchived = true;

            var messageData = new ProductArchivedMessage
            {
                Id = id,
                IsArchived = true
            };

            var message = new Message(messageData);

            await this.dbContext
                .Messages
                .AddAsync(message);

            this.dbContext
                .Products
                .Update(product);

            await this.dbContext
                .SaveChangesAsync();

            await this.publisher.Publish(messageData);

            message.MarkAsPublished();

            await this.dbContext
                            .SaveChangesAsync();

            return this.Ok();
        }

        [HttpPost]
        [Route("[action]" + PathSeparator + Id)]
        [Authorize(Roles = AdminConstants.AdministratorRole)]
        public async Task<ActionResult> Unarchive(int id)
        {
            var product = await this.dbContext
                .Products
                .FindAsync(id);

            if (product == null)
            {
                return this.BadRequest(ProductConstants.ProductDoesNotExistMessage);
            }

            product.IsArchived = false;

            var messageData = new ProductArchivedMessage
            {
                Id = id,
                IsArchived = false
            };

            var message = new Message(messageData);

            await this.dbContext
                .Messages
                .AddAsync(message);

            this.dbContext
                .Products
                .Update(product);

            await this.dbContext
                .SaveChangesAsync();

            var msg = await this.dbContext
                .Messages
                .FindAsync(message.Id);

            await this.publisher.Publish(messageData);

            msg.MarkAsPublished();

            await this.dbContext
                .SaveChangesAsync();

            return this.Ok();
        }
    }
}
