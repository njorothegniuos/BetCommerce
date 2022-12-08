using EmailService;
using Serilog;

IHost host = Host.CreateDefaultBuilder(args)
  .UseSerilog((context, configuration) =>
  {

      configuration
          .MinimumLevel.Information()
          .Enrich.FromLogContext()
          .WriteTo.File(@"C:\Application\MicroServices\Logs\" + DateTime.Now.ToString("yyyyMMdd") + @"\EmailService.log", rollingInterval: RollingInterval.Hour, outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] [{ClientIp}] [{RequestId}] [{RequestPath}] [{Message:lj}] [{Exception}]{NewLine}");
  })
    .ConfigureServices(services =>
    {
        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
        var config = builder.Build();

        var emailConfig = config
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
        services.AddSingleton(emailConfig);
        services.AddScoped<IEmailSender, EmailSender>();
        services.AddHostedService<Worker>();

    })
    .Build();

await host.RunAsync();
