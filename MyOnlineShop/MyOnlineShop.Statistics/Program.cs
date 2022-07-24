using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MyOnlineShop.Common.Infrastructure;
using MyOnlineShop.Common.Services;
using MyOnlineShop.Statistics.Data;
using MyOnlineShop.Statistics.DataSeed;
using MyOnlineShop.Statistics.Messages;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddWebService<StatisticsDbContext>(builder.Configuration)
    .AddAutoMapper(Assembly.GetExecutingAssembly())
    .AddTransient<IDataSeeder, StatisticsDataSeeder>()
    .AddMessaging(builder.Configuration, typeof(ProductCreatedConsumer), typeof(OrderPlacedConsumer));

var app = builder.Build();

app
   .UseWebService(app.Environment)
   .Initialize();

app.Run();