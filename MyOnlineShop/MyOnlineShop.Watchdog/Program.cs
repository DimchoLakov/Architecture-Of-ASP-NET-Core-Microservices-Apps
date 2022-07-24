using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddHealthChecksUI()
    .AddInMemoryStorage();

var app = builder.Build();

app
   .UseRouting()
   .UseEndpoints(endpoints => endpoints.MapHealthChecksUI());

app.Run();