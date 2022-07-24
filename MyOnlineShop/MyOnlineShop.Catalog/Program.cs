using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MyOnlineShop.Catalog.DataSeed;
using MyOnlineShop.Catalog.Filters;
using MyOnlineShop.Common.Infrastructure;
using MyOnlineShop.Common.Services;
using MyOnlineShop.Ordering.Data;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddWebService<CatalogDbContext>(builder.Configuration)
    .AddTransient<IDataSeeder, CatalogDataSeeder>()
    .AddScoped<AddCustomerActionFilter>()
    .AddAutoMapper(Assembly.GetExecutingAssembly())
    .AddMessaging(builder.Configuration);

var app = builder.Build();

app
    .UseWebService(app.Environment)
    .Initialize();

app.Run();