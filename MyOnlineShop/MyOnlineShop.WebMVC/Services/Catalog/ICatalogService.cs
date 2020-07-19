using MyOnlineShop.Common.ViewModels.Addresses;
using MyOnlineShop.Common.ViewModels.Categories;
using MyOnlineShop.Common.ViewModels.Products;
using MyOnlineShop.Common.ViewModels.ShoppingCarts;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyOnlineShop.WebMVC.Services.Catalog
{
    public interface ICatalogService
    {
        [Get("/Products")]
        Task<ProductPaginationViewModel> GetProductPagination([Query] string area, [Query] int? currentPage = 1, [Query] string search = null);

        [Get("/Products/{id}")]
        Task<ProductDetailsViewModel> GetProductDetails(int id, int? fromPage);

        [Get("/Products/GetCreate")]
        Task<CreateProductViewModel> GetCreateProduct();

        [Post("/Products")]
        Task CreateProduct([Body] CreateProductViewModel createProductViewModel);

        [Get("/Products/GetEdit")]
        Task<EditProductViewModel> GetEditProduct([Query] int id, [Query] int? fromPage);

        [Put("/Products")]
        Task EditProduct(EditProductViewModel editProductViewModel);

        [Post("/Products/Archive/{id}")]
        Task ArchiveProduct(int id);

        [Post("/Products/Unarchive/{id}")]
        Task UnarchiveProduct(int id);

        [Get("/Categories")]
        Task<IEnumerable<CategoryIndexViewModel>> GetCategories();

        [Post("/Categories")]
        Task AddCategory(string name);

        [Put("/Categories/{id}")]
        Task ChangeCategoryStatus(int id);
    }
}
