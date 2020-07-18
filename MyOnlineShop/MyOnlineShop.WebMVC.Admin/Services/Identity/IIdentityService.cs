using MyOnlineShop.WebMVC.Admin.ViewModels.Identity;
using Refit;
using System.Threading.Tasks;

namespace MyOnlineShop.WebMVC.Admin.Services.Identity
{
    public interface IIdentityService
    {
        [Post("/Identity/Login")]
        Task<string> Login([Body] LoginCustomerInputModel loginCustomerInputModel);

        [Post("/Identity/Register")]
        Task<string> Register([Body] RegisterCustomerInputModel registerCustomerInputModel);
    }
}
