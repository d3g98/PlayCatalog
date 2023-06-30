using Play.Common.Settings;
using RabbitMQ.Client;

namespace Play.Inventory.Service.Consumers
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly IConfiguration _configuration;
        public RabbitMQService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConnection CreateConnection()
        {
            var rabbitMQ = _configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
            var factory = new ConnectionFactory { HostName = rabbitMQ.Host };
            return factory.CreateConnection();
        }
    }
}
