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
            //Here we specify the Rabbit MQ Server. we use rabbitmq docker image and use it
            var factory = new ConnectionFactory
            {
                HostName = rabbitMQSettings.Host
            };
            //Create the RabbitMQ connection using connection factory details as i mentioned above
            var connection = factory.CreateConnection();
            //Here we create channel with session and model
            using var channel = connection.CreateModel();
            //declare the queue after mentioning name and a few property related to that
            channel.QueueDeclare(message.GetType().ToString(), exclusive: false);
            //Serialize the message
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            //put the data on to the product queue
            channel.BasicPublish(exchange: "", routingKey: message.GetType().ToString(), body: body);
        }
    }
}
