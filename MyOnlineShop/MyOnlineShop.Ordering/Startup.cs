using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyOnlineShop.Common.Infrastructure;
using MyOnlineShop.Ordering.Data;
using System.Reflection;

namespace MyOnlineShop.Ordering
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
                .AddWebService<OrderingDbContext>(this.Configuration)
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
