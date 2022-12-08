namespace RabbitMQ.Services.Interface
{
    public interface IMessageQueueService<TMessage>
    {
        void Send(TMessage data);
    }
}
