namespace RabbitMQ.Utility
{
    public class RabbitMQConfiguration
    {
        public readonly string? _Uri;
        public readonly string? _port;
        public readonly string? _hostName;
        public readonly string? _userName;
        public readonly string? _password;
        public readonly string? _virtualHost;

        RabbitMQConfiguration()
        {

        }
        public RabbitMQConfiguration(string Uri, string port, string hostName, string userName, string password, string virtualHost)
        {
            _Uri = Uri ?? throw new ArgumentNullException(nameof(Uri));
            _port = port ?? throw new ArgumentNullException(nameof(port));
            _hostName = hostName ?? throw new ArgumentNullException(nameof(hostName));
            _userName = userName ?? throw new ArgumentNullException(nameof(userName));
            _password = password ?? throw new ArgumentNullException(nameof(password));
            _virtualHost = virtualHost ?? throw new ArgumentNullException(nameof(virtualHost));
        }
    }
}
