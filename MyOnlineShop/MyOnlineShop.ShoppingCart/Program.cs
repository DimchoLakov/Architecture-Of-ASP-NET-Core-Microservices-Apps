using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MyOnlineShop.Common.Infrastructure;
using MyOnlineShop.Ordering.Data;
using MyOnlineShop.ShoppingCart.Messages;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddWebService<ShoppingCartDbContext>(builder.Configuration)
    .AddAutoMapper(Assembly.GetExecutingAssembly())
    .AddMessaging(builder.Configuration, typeof(ProductUpdatedConsumer), typeof(ProductArchivedConsumer));

var app = builder.Build();

app
    .UseWebService(app.Environment)
    .Initialize();

app.Run();