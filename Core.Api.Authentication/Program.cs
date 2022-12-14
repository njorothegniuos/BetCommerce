using Application.Behaviors;
using Core.Api.Authentication.Middleware;
using Domain.Abstractions;
using FluentValidation;
using Identity;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Build a config object, using env vars and JSON providers.
IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var identityAssembly = typeof(Identity.AssemblyReference).Assembly;

builder.Services.AddControllers()
    .AddApplicationPart(identityAssembly);
builder.Services.AddIdentity(config);


var applicationAssembly = typeof(Application.AssemblyReference).Assembly;

builder.Services.AddMediatR(applicationAssembly);

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddValidatorsFromAssembly(applicationAssembly);




builder.Services.AddInfrastructure(config);

builder.Services.AddScoped<IUnitOfWork>(
            factory => factory.GetRequiredService<ApplicationDbContext>());

builder.Services.AddScoped<IDbConnection>(
    factory => factory.GetRequiredService<ApplicationDbContext>().Database.GetDbConnection());

builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Host.UseSerilog((context, configuration) =>
{

    configuration
        .MinimumLevel.Information()
        .Enrich.FromLogContext()
        .WriteTo.File(@"C:\Application\API\BET\Logs\" + DateTime.Now.ToString("yyyyMMdd") + @"\core.api.log", rollingInterval: RollingInterval.Hour, outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] [{ClientIp}] [{RequestId}] [{RequestPath}] [{Message:lj}] [{Exception}]{NewLine}");
});

var app = builder.Build();

await ApplyMigrations(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}
var provider = app.Services.GetRequiredService<Microsoft.AspNetCore.Mvc.ApiExplorer.IApiVersionDescriptionProvider>();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseSwaggerDocumentation(provider);

app.UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();

static async Task ApplyMigrations(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();

    await using var dbContext = scope.ServiceProvider.GetRequiredService<IdentityContext>();

    await dbContext.Database.MigrateAsync();
}

public static class ConfigurationExtensionMethods
{
    public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app, Microsoft.AspNetCore.Mvc.ApiExplorer.IApiVersionDescriptionProvider provider)
    {
        // Enable middleware to serve generated Swagger as a JSON endpoint.            
        app.UseSwagger();

        //Enable middleware to serve swagger - ui(HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
        app.UseSwaggerUI(options =>
        {
            //options.RoutePrefix = "";
            // build a swagger endpoint for each discovered API version
            foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
            }
            options.DocExpansion(docExpansion: DocExpansion.None);
        });

        return app;
    }
}