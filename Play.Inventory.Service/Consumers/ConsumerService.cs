using Play.Catalog.Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Play.Inventory.Service.Consumers
{
    public abstract class ConsumerService : IConsumerService, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName;
        public ConsumerService(IRabbitMQService rabbitMQService)
        {
            _connection = rabbitMQService.CreateConnection();
            _channel = _connection.CreateModel();

            _queueName = nameof(CatalogItemCreated);

            _channel.QueueDeclare(queue: _queueName,
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
        }

        public async Task ReadMessage()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                if (body.Length != 0)
                {
                    var message = System.Text.Encoding.UTF8.GetString(body);
                    if (message != null && message.Length > 0) await DoWorkAsync(message);
                }
                await Task.CompletedTask;
                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(_queueName, true, consumer);
            await Task.CompletedTask;
        }

        public abstract Task DoWorkAsync(string message);

        public void Dispose()
        {
            if (_channel.IsOpen)
                _channel.Close();
            if (_connection.IsOpen)
                _connection.Close();
        }
    }
}
