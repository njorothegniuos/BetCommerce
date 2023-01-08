namespace RabbitMQ.Utility
{
    public class RabbitMQConfiguration
    {
        public string Uri { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
        public string HostName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string VirtualHost { get; set; } = string.Empty;
        public string EmailAlertRequestPath { get; set; } = string.Empty;
        public string EmailAlertDBLoggerPath { get; set; } = string.Empty;
        public string OrderRequestPath { get; set; } = string.Empty;
    }
}
