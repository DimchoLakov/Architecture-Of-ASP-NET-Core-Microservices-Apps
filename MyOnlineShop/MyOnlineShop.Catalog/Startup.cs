using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyOnlineShop.Catalog.DataSeed;
using MyOnlineShop.Catalog.Filters;
using MyOnlineShop.Common.Infrastructure;
using MyOnlineShop.Common.Services;
using MyOnlineShop.Ordering.Data;
using System.Reflection;

namespace MyOnlineShop.Catalog
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
                .AddWebService<CatalogDbContext>(this.Configuration)
                .AddTransient<IDataSeeder, CatalogDataSeeder>()
                .AddScoped<AddCustomerActionFilter>()
                .AddAutoMapper(Assembly.GetExecutingAssembly())
                .AddMessaging(this.Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app
               .UseWebService(env)
               .Initialize();
        }
    }
}
