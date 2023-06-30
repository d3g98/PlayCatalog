namespace Play.Inventory.Service.Consumers.CatalogItemConsumer
{
    public class DeletedCatalogItemConsumerHostedService : BackgroundService
    {
        private readonly IDeletedCatalogItemConsumerService _consumerService;

        public DeletedCatalogItemConsumerHostedService(IDeletedCatalogItemConsumerService consumerService)
        {
            _consumerService = consumerService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _consumerService.ReadMessage();
        }
    }
}
