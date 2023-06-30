namespace Play.Inventory.Service.Consumers.CatalogItemConsumer
{
    public class UpdatedCatalogItemConsumerHostedService : BackgroundService
    {
        private readonly IUpdatedCatalogItemConsumerService _consumerService;

        public UpdatedCatalogItemConsumerHostedService(IUpdatedCatalogItemConsumerService consumerService)
        {
            _consumerService = consumerService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _consumerService.ReadMessage();
        }
    }
}
