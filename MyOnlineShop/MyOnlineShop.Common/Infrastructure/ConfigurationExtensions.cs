using Microsoft.Extensions.Configuration;

namespace MyOnlineShop.Common.Infrastructure
{
    public static class ConfigurationExtensions
    {
        public static string GetDefaultConnectionString(this IConfiguration configuration)
        {
            return configuration
                        .GetConnectionString("DefaultConnection");
        }
    }
}
