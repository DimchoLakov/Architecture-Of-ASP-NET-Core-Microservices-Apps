using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyOnlineShop.Common.Infrastructure;
using MyOnlineShop.Common.Services;
using MyOnlineShop.ShoppingCart.Gateway.Services;
using MyOnlineShop.ShoppingCart.Gateway.Services.Catalog;
using MyOnlineShop.ShoppingCart.Gateway.Services.Ordering;
using MyOnlineShop.ShoppingCart.Gateway.Services.ShoppingCart;
using Newtonsoft.Json;
using Refit;
using System.Reflection;

namespace MyOnlineShop.ShoppingCart.Gateway
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
            var serviceEndpoints = this.Configuration
                .GetSection(nameof(ServiceEndpoints))
                .Get<ServiceEndpoints>(config => config.BindNonPublicProperties = true);

            services
                .AddAutoMapper(Assembly.GetExecutingAssembly())
                .AddApplicationSettings(this.Configuration)
                .AddRouting()
                .AddJwtTokenAuthentication(this.Configuration)
                .AddScoped<ICurrentTokenService, CurrentTokenService>()
                .AddScoped<ICurrentUserService, CurrentUserService>()
                .AddTransient<JwtHeaderAuthenticationMiddleware>()
                .AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.Formatting = Formatting.Indented;
                });

            services
                .AddRefitClient<ICatalogService>()
                .WithConfiguration(serviceEndpoints.Catalog);

            services
                .AddRefitClient<IOrderingService>()
                .WithConfiguration(serviceEndpoints.Ordering);

            services
                .AddRefitClient<IShoppingCartService>()
                .WithConfiguration(serviceEndpoints.ShoppingCart);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseHttpsRedirection()
                .UseRouting()
                .UseCors(options => options
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod())
                .UseJwtHeaderAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints => endpoints
                    .MapControllers());
        }
    }
}
