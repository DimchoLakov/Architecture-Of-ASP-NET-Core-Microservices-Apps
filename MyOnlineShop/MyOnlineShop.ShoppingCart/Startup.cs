using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyOnlineShop.Common.Infrastructure;
using MyOnlineShop.Ordering.Data;
using MyOnlineShop.ShoppingCart.Messages;
using System.Reflection;

namespace MyOnlineShop.ShoppingCart
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddWebService<ShoppingCartDbContext>(this.Configuration)
                .AddAutoMapper(Assembly.GetExecutingAssembly())
                .AddMessaging(this.Configuration, typeof(ProductUpdatedConsumer), typeof(ProductArchivedConsumer));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app
               .UseWebService(env)
               .Initialize();
        }
    }
}
