namespace MyOnlineShop.Identity.Services.Identity
{
    using Microsoft.AspNetCore.Identity;
    using MyOnlineShop.Identity.Data.Models;
    using MyOnlineShop.Identity.Models;
    using System.Linq;
    using System.Threading.Tasks;

    public class IdentityService : IIdentityService
    {
        private const string InvalidCredentialsErrorMessage = "Invalid credentials!";

        private readonly UserManager<User> userManager;
        private readonly ITokenGeneratorService jwtTokenGenerator;

        public IdentityService(
            UserManager<User> userManager,
            ITokenGeneratorService jwtTokenGenerator)
        {
            this.userManager = userManager;
            this.jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<RegisterCustomerResultModel> RegisterAsync(RegisterCustomerInputModel registerCustomerInputModel)
        {
            var user = new User
            {
                Email = registerCustomerInputModel.Email,
                UserName = registerCustomerInputModel.Email
            };

            var identityResult = await this.userManager.CreateAsync(user, registerCustomerInputModel.Password);

            var registerCustomerResultOutputModel = new RegisterCustomerResultModel();

            if (identityResult.Succeeded)
            {
                registerCustomerResultOutputModel.Succeeded = true;
                registerCustomerResultOutputModel.Email = registerCustomerInputModel.Email;
                registerCustomerResultOutputModel.Password = registerCustomerInputModel.Password;

                return registerCustomerResultOutputModel;
            }

            var errors = identityResult.Errors.Select(e => e.Description);
            registerCustomerResultOutputModel.Succeeded = false;
            registerCustomerResultOutputModel.Errors = errors.ToList();

            return registerCustomerResultOutputModel;
        }

        public async Task<LoginCustomerResultModel> LoginAsync(LoginCustomerInputModel loginCustomerInputModel)
        {
            var loginCustomerResultModel = new LoginCustomerResultModel();

            var user = await this.userManager.FindByEmailAsync(loginCustomerInputModel.Email);
            if (user == null)
            {
                loginCustomerResultModel.Succeeded = false;
                loginCustomerResultModel.Errors.Add(InvalidCredentialsErrorMessage);

                return loginCustomerResultModel;
            }

            var passwordValid = await this.userManager.CheckPasswordAsync(user, loginCustomerInputModel.Password);
            if (!passwordValid)
            {
                loginCustomerResultModel.Succeeded = false;
                loginCustomerResultModel.Errors.Add(InvalidCredentialsErrorMessage);

                return loginCustomerResultModel;
            }

            var roles = await this.userManager.GetRolesAsync(user);

            var token = this.jwtTokenGenerator.GenerateToken(user, roles);

            loginCustomerResultModel.Succeeded = true;
            loginCustomerResultModel.Token = token;

            return loginCustomerResultModel;
        }

        public async Task<ChangePasswordResultModel> ChangePasswordAsync(
            string userId,
            ChangePasswordInputModel changePasswordInput)
        {
            var changePasswordResultModel = new ChangePasswordResultModel();

            var user = await this.userManager.FindByIdAsync(userId);
            if (user == null)
            {
                changePasswordResultModel.Succeeded = false;
                changePasswordResultModel.Errors.Add(InvalidCredentialsErrorMessage);

                return changePasswordResultModel;
            }

            var identityResult = await this.userManager.ChangePasswordAsync(
                user,
                changePasswordInput.CurrentPassword,
                changePasswordInput.NewPassword);

            if (!identityResult.Succeeded)
            {
                changePasswordResultModel.Succeeded = false;

                var errors = identityResult.Errors.Select(e => e.Description).ToList();
                errors.ForEach(error =>
                {
                    changePasswordResultModel.Errors.Add(error);
                });

                return changePasswordResultModel;
            }

            changePasswordResultModel.Succeeded = true;

            return changePasswordResultModel;
        }
    }
}
