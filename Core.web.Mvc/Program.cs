using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace Core.web.Mvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (IServiceScope scope = host.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;
                Microsoft.Extensions.Logging.ILogger logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                try
                {
                    IdentityContext context = services.GetRequiredService<IdentityContext>();
                    if (context.Database.IsSqlServer()) { context.Database.Migrate(); }
                }
                catch (Exception)
                {
                    logger.LogError("An error while setting up infrastructure - migration, sequences and seed");
                    throw;
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
          Host.CreateDefaultBuilder(args)
            .UseSerilog((context, configuration) =>
            {

                configuration
                    .MinimumLevel.Information()
                    .Enrich.FromLogContext()
                    .WriteTo.File(@"E:\BET\Application\Core.Web.mVC\Logs\" + DateTime.Now.ToString("yyyyMMdd") + @"\Core.Web.mVC.log", rollingInterval: RollingInterval.Hour, outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] [{ClientIp}] [{RequestId}] [{RequestPath}] [{Message:lj}] [{Exception}]{NewLine}");
            })
              .ConfigureLogging((hostingContext, logging) =>
              {
                  logging.ClearProviders();
                  logging.AddDebug();
              })
              .ConfigureWebHostDefaults(webBuilder =>
              {


                  webBuilder.UseStartup<Startup>();
              });
    }
}
