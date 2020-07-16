using MyOnlineShop.Common.ViewModels.Images;
using Refit;
using System.Threading.Tasks;

namespace MyOnlineShop.WebMVC.Services.Images
{
    public interface IImageService
    {
        [Get("/Images/{id}")]
        Task<ImageViewModel> Get(int id);

        [Delete("/Images/{id}")]
        Task Delete(int id);

        [Post("/Images/{productId}")]
        Task AddToProduct(int productId, [Body]AddImageViewModel addImageViewModel);
    }
}
