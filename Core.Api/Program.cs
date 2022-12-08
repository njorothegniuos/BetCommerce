using Application.Behaviors;
using Core.Api.Middleware;
using Domain.Abstractions;
using FluentValidation;
using Identity;
using Infrastructure;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using System.Data;
using System.Reflection;

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
builder.Services.AddIdentityService(config);
builder.Services.AddTransient<IdentityContext>();

var presentationAssembly = typeof(Presentation.AssemblyReference).Assembly;

builder.Services.AddControllers()
    .AddApplicationPart(presentationAssembly);

var applicationAssembly = typeof(Application.AssemblyReference).Assembly;

builder.Services.AddMediatR(applicationAssembly);

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddValidatorsFromAssembly(applicationAssembly);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen(c =>
{
    var presentationDocumentationFile = $"{presentationAssembly.GetName().Name}.xml";

    var presentationDocumentationFilePath =
        Path.Combine(AppContext.BaseDirectory, presentationDocumentationFile);

    c.IncludeXmlComments(presentationDocumentationFilePath);

    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Core.Api", Version = "v1" });
});
builder.Services.AddInfrastructure(config);

builder.Services.AddScoped<IUnitOfWork>(
            factory => factory.GetRequiredService<ApplicationDbContext>());

builder.Services.AddScoped<IDbConnection>(
    factory => factory.GetRequiredService<ApplicationDbContext>().Database.GetDbConnection());

builder.Services.AddTransient<ExceptionHandlingMiddleware>();

var app = builder.Build();

await ApplyMigrations(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web v1"));
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();

static async Task ApplyMigrations(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();

    await using var dbContext = scope.ServiceProvider.GetRequiredService<IdentityContext>();

    await dbContext.Database.MigrateAsync();
    //Uncomment to seed user login data
    //DataInitializerConfiguration dataInitializerConfiguration = new DataInitializerConfiguration(dbContext);

    using var _scope  = serviceProvider.CreateScope();

    await using var _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await dbContext.Database.MigrateAsync();
}