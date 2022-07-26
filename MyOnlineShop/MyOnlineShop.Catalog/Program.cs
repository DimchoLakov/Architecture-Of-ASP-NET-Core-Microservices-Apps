using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MyOnlineShop.Catalog.DataSeed;
using MyOnlineShop.Catalog.Messages;
using MyOnlineShop.Common.Infrastructure;
using MyOnlineShop.Common.Services;
using MyOnlineShop.Ordering.Data;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddWebService<CatalogDbContext>(builder.Configuration)
    .AddTransient<IDataSeeder, CatalogDataSeeder>()
    .AddAutoMapper(Assembly.GetExecutingAssembly())
    .AddMessaging(builder.Configuration, typeof(UserCreatedMessageConsumer));

var app = builder.Build();

app
    .UseWebService(app.Environment)
    .Initialize();

app.Run();