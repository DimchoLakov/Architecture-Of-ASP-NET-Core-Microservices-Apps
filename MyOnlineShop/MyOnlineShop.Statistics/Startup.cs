using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyOnlineShop.Common.Infrastructure;
using MyOnlineShop.Common.Services;
using MyOnlineShop.Statistics.Data;
using MyOnlineShop.Statistics.DataSeed;
using MyOnlineShop.Statistics.Messages;
using System.Reflection;

namespace MyOnlineShop.Statistics
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
                .AddWebService<StatisticsDbContext>(this.Configuration)
                .AddAutoMapper(Assembly.GetExecutingAssembly())
                .AddTransient<IDataSeeder, StatisticsDataSeeder>()
                .AddMessaging(this.Configuration, typeof(ProductCreatedConsumer), typeof(OrderPlacedConsumer));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app
               .UseWebService(env)
               .Initialize();
        }
    }
}
