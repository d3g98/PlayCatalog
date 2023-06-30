using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Play.Common.Settings;
using RabbitMQ.Client;
using System.Text;

namespace Play.Catalog.Serivce.RabbitMQ
{
    public class RabitMQProducer : IRabitMQProducer
    {
        private readonly IConfiguration _configuration;
        public RabitMQProducer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendMessage<T>(T message)
        {
            var rabbitMQSettings = _configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
            var factory = new ConnectionFactory
            {
                HostName = rabbitMQSettings.Host
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    string queueName = message.GetType().Name;
                    channel.QueueDeclare(queue: queueName,
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

                    var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                    channel.BasicPublish(exchange: string.Empty,
                                         routingKey: queueName,
                                         basicProperties: null,
                                         body: body);
                }
            }
        }
    }
}
