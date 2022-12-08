using RabbitMQ.Utility;

namespace EmailService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private EmailQueueProcessor _emailQueueProcessor;
        private readonly RabbitMQConfiguration _rabbitMQConfiguration;

        public Worker(ILogger<Worker> logger, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            IServiceScope scope = _serviceProvider.CreateScope();
            _emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();
            _rabbitMQConfiguration = new RabbitMQConfiguration(_configuration["RabbitMqQueueSettings:Uri"],
                              _configuration["RabbitMqQueueSettings:Port"], _configuration["RabbitMqQueueSettings:HostName"],
                              _configuration["RabbitMqQueueSettings:UserName"], _configuration["RabbitMqQueueSettings:Password"],
                              _configuration["RabbitMqQueueSettings:VirtualHost"]);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Serilog.Log.Information("Worker running at: {time}", DateTimeOffset.Now);

            _emailQueueProcessor = new EmailQueueProcessor(_emailSender,_configuration["RabbitMqQueueSettings:EmailRequestPath"], consumers: 5, _rabbitMQConfiguration);

            _emailQueueProcessor.Open();

            await Task.CompletedTask;
        }
    }
}