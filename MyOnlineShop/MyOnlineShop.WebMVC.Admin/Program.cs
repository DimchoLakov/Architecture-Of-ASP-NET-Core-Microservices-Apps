using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyOnlineShop.Common.Infrastructure;
using MyOnlineShop.Common.Services;
using MyOnlineShop.WebMVC.Admin.Services;
using MyOnlineShop.WebMVC.Admin.Services.Catalog;
using MyOnlineShop.WebMVC.Admin.Services.Identity;
using MyOnlineShop.WebMVC.Admin.Services.Statistics;
using Newtonsoft.Json;
using Refit;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

var serviceEndpoints = builder
    .Configuration
    .GetSection(nameof(ServiceEndpoints))
    .Get<ServiceEndpoints>(config => config.BindNonPublicProperties = true);

builder
    .Services
    .AddRouting()
    .AddAutoMapper(Assembly.GetExecutingAssembly())
    .AddJwtTokenAuthentication(builder.Configuration)
    .AddScoped<ICurrentTokenService, CurrentTokenService>()
    .AddScoped<ICurrentUserService, CurrentUserService>()
    .AddTransient<JwtCookieAuthenticationMiddleware>()
    .AddDatabaseDeveloperPageExceptionFilter();

builder
    .Services
    .AddRefitClient<IIdentityService>()
    .WithConfiguration(serviceEndpoints.Identity);

builder
    .Services
    .AddRefitClient<ICatalogService>()
    .WithConfiguration(serviceEndpoints.Catalog);

builder
    .Services
    .AddRefitClient<IStatisticsService>()
    .WithConfiguration(serviceEndpoints.Statistics);

builder
    .Services
    .AddRazorPages();

builder
    .Services
    .AddControllersWithViews(options =>
    {
        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
    })
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.Formatting = Formatting.Indented;
    });

var app = builder.Build();

if (app
    .Environment
    .IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app
    .UseHttpsRedirection()
    .UseStaticFiles()
    .UseStatusCodePages(async context =>
        await Task.Run(() =>
        {
            var request = context.HttpContext.Request;
            var response = context.HttpContext.Response;

            if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                response.Redirect("/Identity/Login");
            }

            if (response.StatusCode == (int)HttpStatusCode.NotFound)
            {
                response.Redirect("/Home/Error");
            }
        }))
    .UseRouting()
    .UseJwtCookieAuthentication()
    .UseAuthorization()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Products}/{action=Index}/{id?}");
    });

app.Run();