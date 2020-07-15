namespace MyOnlineShop.Common.Services
{
    public interface ICurrentUserService
    {
        public string UserId { get; }

        public string Username { get; }

        public bool IsAdministrator { get; }
    }
}
