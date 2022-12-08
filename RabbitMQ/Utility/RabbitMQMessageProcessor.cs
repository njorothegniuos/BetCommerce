using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace RabbitMQ.Utility
{
    public abstract class RabbitMQMessageProcessor<TMessage>
    {
        private readonly string _path;
        private readonly IModel _channel;
        private readonly IConnection _connection;
        private readonly IConnectionFactory _connectionFactory;
        private readonly AsyncEventingBasicConsumer[] _receivers;
        private readonly Counter ProcessingCounter = new Counter();
        public bool IsOpen { get; private set; }
        protected bool IsProcessing
        {
            get { return this.ProcessingCounter.Value > 0; }
        }
        private bool IsClosing;
        private RabbitMQMessageProcessor()
        { }

        public RabbitMQMessageProcessor(string path, RabbitMQConfiguration rabbitMQConfiguration)
            : this(path, 1, rabbitMQConfiguration)
        { }

        public RabbitMQMessageProcessor(string path, int count, RabbitMQConfiguration rabbitMQConfiguration)
            : base()
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
                Uri = new Uri(rabbitMQConfiguration._Uri), //"amqp://user:pass@hostName:port/vhost";
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

            //This tells RabbitMQ not to give more than one message to a worker at a time. Or,
            //in other words, don't dispatch a new message to a worker until it has processed and acknowledged the previous one.
            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            //create receivers i.e instances of workers
            this._receivers = Enumerable.Range(0, (count < 1) ? 1 : count)
               .Select(i =>
               {
                   AsyncEventingBasicConsumer queue = new AsyncEventingBasicConsumer(_channel);

                   return queue;
               }).ToArray();

        }

        protected abstract Task Process(TMessage @object);
        protected abstract void LogError(Exception exception);
        protected virtual void OnClosing() { }
        protected virtual void OnClosed() { }
        protected virtual void OnOpening() { }
        protected virtual void OnOpened() { }

        /// <summary>
        /// Call this method after creating an instance of the message processor in order to add event listners
        /// </summary>
        public void Open()
        {
            if (this.IsOpen)
                throw new Exception("This processor is already open.");

            this.OnOpening();

            foreach (AsyncEventingBasicConsumer receiver in _receivers)
            {
                //add event listeners 
                receiver.Received += Queue_Received;

                //start consumer and dont acknowledge receipt of message automatically
                _channel.BasicConsume(queue: _path,
                                     autoAck: false,
                                     consumer: receiver);
            }

            this.IsOpen = true;
            this.OnOpened();
        }

        private async Task Queue_Received(object sender, BasicDeliverEventArgs e)
        {
            try
            {
                byte[] body = e.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);

                this.Handle(message);
                //manually acknowledge message has been processed when done
                _channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {

                this.LogError(ex);

            }
            finally
            {
            }
        }

        private void Handle(string message)
        {
            if (string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message)) return;

            this.ProcessingCounter.Increment();

            try
            {
                Task workerTask = Process(JsonSerializer.Deserialize<TMessage>(message));

                Task.WaitAll(workerTask);
            }
            catch (Exception ex)
            {
                this.LogError(ex);
            }
            finally
            {
                this.ProcessingCounter.Decrement();
            }
        }

        public void Close()
        {
            this.IsClosing = true;

            this.OnClosing();

            foreach (AsyncEventingBasicConsumer queue in this._receivers)
            {
                queue.Received -= Queue_Received;
            }


            while (this.IsProcessing)
                Thread.Sleep(100);

            _channel.Close();

            this.IsClosing = this.IsOpen = false;
            this.OnClosed();
        }
    }
}
