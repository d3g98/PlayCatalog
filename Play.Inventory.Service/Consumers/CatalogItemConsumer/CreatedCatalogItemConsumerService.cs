using Newtonsoft.Json;
using Play.Catalog.Contracts;
using Play.Common;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers.CatalogItemConsumer
{
    public class CreatedCatalogItemConsumerService : ConsumerService, ICreatedCatalogItemConsumerService
    {
        private readonly IRepository<CatalogItem> _repository;
        public CreatedCatalogItemConsumerService(IRabbitMQService rabbitMQService, IRepository<CatalogItem> repository)
            : base(rabbitMQService)
        {
            _repository = repository;
        }

        public override async Task DoWorkAsync(string message)
        {
            var model = JsonConvert.DeserializeObject<CatalogItemCreated>(message);
            if (model == null) return;

            var item = await _repository.GetAsync(model.Id);
            if (item != null) return;

            item = new CatalogItem()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description
            };

            await _repository.CreateAsync(item);
        }
    }
}
