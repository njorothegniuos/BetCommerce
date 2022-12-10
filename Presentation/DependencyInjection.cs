using Domain.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swagger.Models.Attribute;
using Swagger.Models.Common;
using Swagger.Models.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Presentation
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddVersioning();
            services.AddSwaggerDocumentation();
            services.AddCustomOptions(configuration);

            return services;
        }
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
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
                var presentationAssembly = typeof(Presentation.AssemblyReference).Assembly;
                var presentationDocumentationFile = $"{presentationAssembly.GetName().Name}.xml";

                var presentationDocumentationFilePath =
                    Path.Combine(AppContext.BaseDirectory, presentationDocumentationFile);

                return presentationDocumentationFilePath;
            }
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
}
