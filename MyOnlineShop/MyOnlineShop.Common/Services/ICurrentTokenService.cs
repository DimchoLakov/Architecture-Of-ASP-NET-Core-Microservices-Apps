namespace MyOnlineShop.Common.Services
{
    public interface ICurrentTokenService
    {
        string Get();

        void Set(string token);
    }
}
