using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(MyOnlineShop.Areas.Identity.IdentityHostingStartup))]
namespace MyOnlineShop.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}