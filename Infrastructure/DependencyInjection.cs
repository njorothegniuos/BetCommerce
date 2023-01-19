using Application.Abstractions;
using Domain.Abstractions;
using Infrastructure.Authentication;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
           services.AddDbContext<ApplicationDbContext>(options =>
                options.EnableSensitiveDataLogging(true).UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sqlServerOptionsAction: sqlOptions =>
                {
                    options.EnableSensitiveDataLogging(false);
                    sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).GetTypeInfo().Assembly.GetName().Name);
                }));

            services.AddScoped<IEmailRepository, EmailRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            return services;
        }
    }
}
