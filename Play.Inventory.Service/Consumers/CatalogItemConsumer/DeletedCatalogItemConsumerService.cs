using Newtonsoft.Json;
using Play.Catalog.Contracts;
using Play.Common;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers.CatalogItemConsumer
{
    public class DeletedCatalogItemConsumerService : ConsumerService, IDeletedCatalogItemConsumerService
    {
        private readonly IRepository<CatalogItem> _repository;
        public DeletedCatalogItemConsumerService(IRabbitMQService rabbitMQService, IRepository<CatalogItem> repository)
            : base(rabbitMQService)
        {
            _repository = repository;
        }

        public override async Task DoWorkAsync(string message)
        {
            var model = JsonConvert.DeserializeObject<CatalogItemDeleted>(message);
            if (model == null) return;

            var item = await _repository.GetAsync(model.Id);
            if (item != null)
            {
                await _repository.DeleteAsync(item.Id);
            }
        }
    }
}
