using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MyOnlineShop.Common.Infrastructure;
using MyOnlineShop.Common.Services;
using MyOnlineShop.Identity.Data;
using MyOnlineShop.Identity.DataSeed;
using MyOnlineShop.Identity.Infrastructure;
using MyOnlineShop.Identity.Services.Identity;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddWebService<MyIdentityDbContext>(builder.Configuration)
    .AddUserStorage()
    .AddTransient<IDataSeeder, IdentityDataSeeder>()
    .AddTransient<IIdentityService, IdentityService>()
    .AddTransient<ITokenGeneratorService, TokenGeneratorService>()
    .AddMessaging(builder.Configuration);

var app = builder.Build();

app
    .UseWebService(app.Environment)
    .Initialize();

app.Run();