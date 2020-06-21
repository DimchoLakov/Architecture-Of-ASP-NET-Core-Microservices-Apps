using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyOnlineShop.Data;
using MyOnlineShop.ViewModels.Pagination;
using MyOnlineShop.ViewModels.Products;
using System;
using System.Linq;
using System.Threading.Tasks;
using static MyOnlineShop.Constants.ProductConstants;

namespace MyOnlineShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public ProductsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IActionResult> Index(int? currentPage = 1, string search = null)
        {
            int count = 0;

            var query = this.dbContext
                .Products
                .Where(x => !x.IsArchived);
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
                .Select(x => new IndexViewModel()
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
                return this.BadRequest(ProductDoesNotExistMessage);
            }

            var productDetailsViewModel = await this.dbContext
                .Products
                .Where(x => x.Id == id)
                .Select(x => new DetailsViewModel
                {
                    Id = x.Id,
                    Description = x.Description,
                    Name = x.Name,
                    Price = x.Price,
                    StockAvailable = x.StockAvailable,
                    Weight = x.Weight,
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
    }
}
