using Application.Email.DTOs;
using RabbitMQ.Utility;
using System.Text.Json;

namespace EmailService
{
    public  class EmailQueueProcessor : RabbitMQMessageProcessor<EmailRequest>
    {
        private readonly IEmailSender _emailSender;
        public EmailQueueProcessor(IEmailSender emailSender,string path, int consumers, RabbitMQConfiguration rabbitMQConfiguration) : base(path, consumers, rabbitMQConfiguration)
        {
            _emailSender = emailSender;
        }


        protected override void LogError(Exception exception)
        {
            Serilog.Log.Error("EmailService.Queue => EmailQueueProcessor: " + exception.Message);
        }
        protected override async Task Process(EmailRequest @object)
        {
            Serilog.Log.Information($"EmailService {JsonSerializer.Serialize(@object)}");

            var message = new Message(new string[] { @object.To }, @object.Subject, @object.Content, null);
            await _emailSender.SendEmailAsync(message);


        }

    }
}
