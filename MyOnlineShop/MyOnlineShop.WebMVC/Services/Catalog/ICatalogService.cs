using MyOnlineShop.Common.ViewModels.Addresses;
using MyOnlineShop.Common.ViewModels.Images;
using MyOnlineShop.Common.ViewModels.Products;
using MyOnlineShop.Common.ViewModels.ShoppingCarts;
using Refit;
using System.Threading.Tasks;

namespace MyOnlineShop.WebMVC.Services.Catalog
{
    public interface ICatalogService
    {
        [Get("/Products")]
        Task<ProductPaginationViewModel> GetProductPagination([Query] int? currentPage = 1, [Query] string search = null);

        [Get("/Products/{id}")]
        Task<ProductDetailsViewModel> GetProductDetails([Query] int id, [Query] int? fromPage);

        [Get("/Products/GetCreate")]
        Task<CreateProductViewModel> GetCreateProduct();

        [Post("/Products")]
        Task CreateProduct(CreateProductViewModel createProductViewModel);

        [Get("/Products/GetEdit")]
        Task<EditProductViewModel> GetEditProduct();

        [Put("/Products")]
        Task EditProduct(EditProductViewModel editProductViewModel);

        [Post("/Products/Archive/{id}")]
        Task ArchiveProduct(int id);

        [Get("/Images/{id}")]
        Task<ImageViewModel> GetImage(int id);

        [Delete("/Images/{id}")]
        Task DeleteImage(int id);

        [Post("/Images/{productId}")]
        Task AddImageToProduct(int productId, [Body] AddImageViewModel addImageViewModel);

        [Get("/Addresses/{userId}")]
        Task<AddressViewModel> GetAddress(string userId);

        [Post("/Addresses/{userId}")]
        Task<int> CreateAddress([Query] string userId, OrderAddressViewModel orderAddressViewModel);
    }
}
