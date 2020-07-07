namespace MyOnlineShop.Identity.Services.Identity
{
    using MyOnlineShop.Identity.Models;
    using System.Threading.Tasks;

    public interface IIdentityService
    {
        Task<RegisterCustomerResultModel> RegisterAsync(RegisterCustomerInputModel registerCustomerModel);

        Task<LoginCustomerResultModel> LoginAsync(LoginCustomerInputModel loginCustomerModel);

        Task<ChangePasswordResultModel> ChangePasswordAsync(string userId, ChangePasswordInputModel changePasswordInput);
    }
}
