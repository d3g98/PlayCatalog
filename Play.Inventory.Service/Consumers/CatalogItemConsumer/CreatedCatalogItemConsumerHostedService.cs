namespace Play.Inventory.Service.Consumers.CatalogItemConsumer
{
    public class CreatedCatalogItemConsumerHostedService : BackgroundService
    {
        private readonly ICreatedCatalogItemConsumerService _consumerService;

        public CreatedCatalogItemConsumerHostedService(ICreatedCatalogItemConsumerService consumerService)
        {
            _consumerService = consumerService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _consumerService.ReadMessage();
        }
    }
}
