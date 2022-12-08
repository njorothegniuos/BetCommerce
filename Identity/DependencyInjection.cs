using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Identity;
using System.Reflection;

namespace Identity
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<IdentityContext>(options =>
                options.EnableSensitiveDataLogging(true).UseSqlServer(configuration.GetConnectionString("AuthConnection"),
                sqlServerOptionsAction: sqlOptions =>
                {
                    options.EnableSensitiveDataLogging(false);
                    sqlOptions.MigrationsAssembly(typeof(IdentityContext).GetTypeInfo().Assembly.GetName().Name);
                }));

            services.AddIdentity<ApplicationUser, IdentityRole>(
                  options =>
                  {
                      options.SignIn.RequireConfirmedPhoneNumber = false;
                      options.SignIn.RequireConfirmedEmail = false;
                  }
              ).AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            return services;
        }
    }
}
