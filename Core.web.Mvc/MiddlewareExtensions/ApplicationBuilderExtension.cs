using Core.Web.SubscribeTableDependencies;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Web.MiddlewareExtensions
{
    public static class ApplicationBuilderExtension
    {
        public static void UseSqlTableDependency<T>(this Microsoft.AspNetCore.Builder.IApplicationBuilder applicationBuilder, string connectionString)
           where T : ISubscribeTableDependency
        {
            var serviceProvider = applicationBuilder.ApplicationServices;
            var service = serviceProvider.GetService<T>();
            service.SubscribeTableDependency(connectionString);
        }
    }
}
