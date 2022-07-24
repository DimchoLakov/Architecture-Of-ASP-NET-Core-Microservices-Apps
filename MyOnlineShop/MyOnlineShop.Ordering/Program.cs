using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MyOnlineShop.Common.Infrastructure;
using MyOnlineShop.Ordering.Data;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddWebService<OrderingDbContext>(builder.Configuration)
    .AddAutoMapper(Assembly.GetExecutingAssembly())
    .AddMessaging(builder.Configuration);

var app = builder.Build();

app
    .UseWebService(app.Environment)
    .Initialize();

app.Run();