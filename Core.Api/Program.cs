using Identity;
using Identity.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Build a config object, using env vars and JSON providers.
IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var identityAssembly = typeof(AssemblyReference).Assembly;

builder.Services.AddControllers()
    .AddApplicationPart(identityAssembly);
builder.Services.AddIdentityService(config);
builder.Services.AddTransient<IdentityContext>();

var app = builder.Build();

await ApplyMigrations(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static async Task ApplyMigrations(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();

    await using var dbContext = scope.ServiceProvider.GetRequiredService<IdentityContext>();

    await dbContext.Database.MigrateAsync();
    //Uncomment to seed user login data
    //DataInitializerConfiguration dataInitializerConfiguration = new DataInitializerConfiguration(dbContext);

}