using Application.Behaviors;
using Core.Api.Middleware;
using Domain.Abstractions;
using Domain.Common;
using FluentValidation;
using Identity;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Presentation.Models.Attribute;
using Presentation.Models.Common;
using Serilog;
using Swagger.Models.Common;
using Swagger.Models.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Data;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

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
builder.Services.AddControllers()
           .AddXmlSerializerFormatters();
builder.Services.AddCustomControllers();
builder.Services.AddVersioning();
builder.Services.AddSwaggerDocumentation();
//services.AddCustomHangfire(Configuration);
builder.Services.AddCustomOptions(config);

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
var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

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
    //Uncomment to seed user login data
    //DataInitializerConfiguration dataInitializerConfiguration = new DataInitializerConfiguration(dbContext);

    using var _scope = serviceProvider.CreateScope();

    await using var _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await dbContext.Database.MigrateAsync();
}

public static class ConfigurationExtensionMethods
{
    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        //Add Token Validation Parameters
        TokenValidationParameters tokenParameters = new()
        {
            //what to validate
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            //set up validation data
            ValidIssuer = configuration["Security:Issuer"],
            ValidAudience = configuration["Security:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Security:Key"])),
            ClockSkew = new TimeSpan(0)//The validation parameters have a default clock skew of 5 minutes so i have to invalidate it to 0
        };

        //Add JWT Authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = tokenParameters;
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(nameof(AuthPolicy.GlobalRights), policy => policy.RequireRole(nameof(Roles.Root), nameof(Roles.Admin), nameof(Roles.Webapi), nameof(Roles.Regular)));
            options.AddPolicy(nameof(AuthPolicy.ElevatedRights), policy => policy.RequireRole(nameof(Roles.Root), nameof(Roles.Admin)));
        });

        return services;
    }

    public static IServiceCollection AddVersioning(this IServiceCollection services)
    {
        //REF https://dev.to/99darshan/restful-web-api-versioning-with-asp-net-core-1e8g
        //REF https://github.com/Microsoft/aspnet-api-versioning/wiki
        services.AddApiVersioning(options =>
        {
            // specify the default API Version as 1.0
            options.DefaultApiVersion = new ApiVersion(1, 0);

            // if the client hasn't specified the API version in the request, use the default API version number 
            options.AssumeDefaultVersionWhenUnspecified = true;

            // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
            options.ReportApiVersions = true;

            // DEFAULT Version reader is QueryStringApiVersionReader();
            // clients request the specific version using the X-version header
            // options.ApiVersionReader = new Microsoft.AspNetCore.Mvc.Versioning.HeaderApiVersionReader("X-version");   
            // Supporting multiple versioning scheme
            // options.ApiVersionReader = ApiVersionReader.Combine(new HeaderApiVersionReader(new[] { "api-version", "x-version", "version" }),
            // new QueryStringApiVersionReader(new[] { "api-version", "v", "version" }));//MediaTypeApiVersionReader-UrlSegmentApiVersionReader

            options.ApiVersionReader = new UrlSegmentApiVersionReader();
            options.ErrorResponses = new VersionErrorProvider();
        });

        services.AddVersionedApiExplorer(options =>
        {
            // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
            // note: the specified format code will format the version as "'v'major[.minor][-status]"
            options.GroupNameFormat = "'v'VVV";

            // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
            // can also be used to control the format of the API version in route templates
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }

    public static IServiceCollection AddCustomControllers(this IServiceCollection services)
    {
        //TODO: revisit porting to native system.text.json https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to
        services.AddControllers(options =>
        {
            options.Filters.Add(typeof(ModelStateFilter));
        })
        .ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        })
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.Converters.Add(new EmptyStringToNullConverter());
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        });

        return services;
    }

    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

        services.AddSwaggerGen(options =>
        {
            // add a custom operation filter which sets default values
            options.OperationFilter<SwaggerDefaultValues>();

            // integrate xml comments
            options.IncludeXmlComments(XmlCommentsFilePath);

            //define how the API is secured by defining one or more security schemes.
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. Enter in the value field: <b>Bearer {your JWT token}</b>"
            });

            options.OrderActionsBy(description =>
            {
                ControllerActionDescriptor controllerActionDescriptor = (ControllerActionDescriptor)description.ActionDescriptor;
                SwaggerOrderAttribute attribute = (SwaggerOrderAttribute)controllerActionDescriptor.ControllerTypeInfo.GetCustomAttribute(typeof(SwaggerOrderAttribute));
                return string.IsNullOrEmpty(attribute?.Order?.Trim()) ? description.GroupName : attribute.Order.Trim();
            });

            //Operation security scheme based on Authorize attribute using OperationFilter()
            options.OperationFilter<SwaggerAuthOperationFilter>();
        });

        return services;
    }

    public static string XmlCommentsFilePath
    {
        get
        {
            //typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
            return Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
        }
    }

    public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
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

    public static IServiceCollection AddCustomOptions(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }


}

public class EmptyStringToNullConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string value = reader.GetString();
        return value == string.Empty ? null : value.Trim();
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}