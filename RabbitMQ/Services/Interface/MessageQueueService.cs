using RabbitMQ.Client;
using RabbitMQ.Utility;
using System.Text;
using System.Text.Json;

namespace RabbitMQ.Services.Interface
{
    public class MessageQueueService<TMessage> : IMessageQueueService<TMessage>
    {
        private readonly string _path;
        private readonly IModel _channel;
        private readonly IConnection _connection;
        private readonly IBasicProperties _properties;
        private readonly IConnectionFactory _connectionFactory;
        public MessageQueueService(string path, RabbitMQConfiguration rabbitMQConfiguration)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            //assign queue name
            _path = path;

            //Get connection factory
            _connectionFactory = new ConnectionFactory

            {
                HostName = rabbitMQConfiguration._hostName,
                Port = Convert.ToInt32(rabbitMQConfiguration._port),
                UserName = rabbitMQConfiguration._userName,
                Password = rabbitMQConfiguration._password,
                VirtualHost = rabbitMQConfiguration._virtualHost,
                Uri = new Uri(rabbitMQConfiguration._Uri),
                DispatchConsumersAsync = true

            };

            //use factory to create connection
            _connection = _connectionFactory.CreateConnection();
            //creates connection session
            _channel = _connection.CreateModel();

            //declare queue and make it durable
            _channel.QueueDeclare(queue: _path,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            _properties = _channel.CreateBasicProperties();
            _properties.Persistent = true;
        }
        public void Send(TMessage data)
        {
            string message = JsonSerializer.Serialize<TMessage>(data);

            byte[] body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "",
                                routingKey: _path,
                                basicProperties: _properties,
                                body: body);
        }
    }
}
