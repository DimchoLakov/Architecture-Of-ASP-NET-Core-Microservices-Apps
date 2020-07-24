using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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

namespace MyOnlineShop.WebMVC.Admin
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
                .AddRouting()
                .AddAutoMapper(Assembly.GetExecutingAssembly())
                .AddJwtTokenAuthentication(this.Configuration)
                .AddScoped<ICurrentTokenService, CurrentTokenService>()
                .AddScoped<ICurrentUserService, CurrentUserService>()
                .AddTransient<JwtCookieAuthenticationMiddleware>();

            services
                .AddRefitClient<IIdentityService>()
                .WithConfiguration(serviceEndpoints.Identity);

            services
                .AddRefitClient<ICatalogService>()
                .WithConfiguration(serviceEndpoints.Catalog);

            services
                .AddRefitClient<IStatisticsService>()
                .WithConfiguration(serviceEndpoints.Statistics);

            services.AddRazorPages();
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.Formatting = Formatting.Indented;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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
        }
    }
}
