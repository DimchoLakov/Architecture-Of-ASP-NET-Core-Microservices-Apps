using Microsoft.AspNetCore.Builder;
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

var builder = WebApplication.CreateBuilder(args);

var serviceEndpoints = builder
    .Configuration
    .GetSection(nameof(ServiceEndpoints))
    .Get<ServiceEndpoints>(config => config.BindNonPublicProperties = true);

builder
    .Services
    .AddAutoMapper(Assembly.GetExecutingAssembly())
    .AddApplicationSettings(builder.Configuration)
    .AddRouting()
    .AddJwtTokenAuthentication(builder.Configuration)
    .AddScoped<ICurrentTokenService, CurrentTokenService>()
    .AddScoped<ICurrentUserService, CurrentUserService>()
    .AddTransient<JwtHeaderAuthenticationMiddleware>()
    .AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.Formatting = Formatting.Indented;
    });

builder
    .Services
    .AddRefitClient<ICatalogService>()
    .WithConfiguration(serviceEndpoints.Catalog);

builder
    .Services
    .AddRefitClient<IOrderingService>()
    .WithConfiguration(serviceEndpoints.Ordering);

builder
    .Services
    .AddRefitClient<IShoppingCartService>()
    .WithConfiguration(serviceEndpoints.ShoppingCart);

var app = builder.Build();

if (app.Environment.IsDevelopment())
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

app.Run();