using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyOnlineShop.Common.Services;
using System;
using Hangfire;
using MyOnlineShop.Common.Messages;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;

namespace MyOnlineShop.Common.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseWebService(this IApplicationBuilder app, IWebHostEnvironment env)
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
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapHealthChecks("/health", new HealthCheckOptions
                    {
                        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                    });

                    endpoints.MapControllers();
                });

            if (app.ApplicationServices.GetService<MessagesHostedService>() != null)
            {
                app.UseHangfireServer();
                app.UseHangfireDashboard();
            }

            return app;
        }

        public static IApplicationBuilder Initialize(this IApplicationBuilder app)
        {
            using var serviceScope = app
                .ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();

            var serviceProvider = serviceScope.ServiceProvider;

            var dbContext = serviceProvider.GetService<DbContext>();

            dbContext.Database.Migrate();

            var dataSeeders = serviceProvider.GetServices<IDataSeeder>();

            foreach (var seeder in dataSeeders)
            {
                seeder.SeedData();
            }

            return app;
        }
    }
}
