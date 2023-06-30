using RabbitMQ.Client;

namespace Play.Inventory.Service.Consumers
{
    public interface IRabbitMQService
    {
        IConnection CreateConnection();
    }
}