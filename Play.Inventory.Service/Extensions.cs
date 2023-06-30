using Play.Inventory.Service.Consumers;
using Play.Inventory.Service.Consumers.CatalogItemConsumer;
using Play.Inventory.Service.Dtos;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service
{
    public static class Extensions
    {
        public static InventoryItemDto AsDto(this InventoryItem item, string name, string description)
        {
            return new InventoryItemDto(item.CatalogItemId, name, description, item.Quantity, item.AcquiredDate);
        }

        public static IServiceCollection AddRabbitMQConsumer(this IServiceCollection services)
        {
            //IRabbitMQService
            services.AddSingleton<IRabbitMQService, RabbitMQService>();

            //ConsumerService
            services.AddSingleton<ICreatedCatalogItemConsumerService, CreatedCatalogItemConsumerService>();
            services.AddSingleton<IUpdatedCatalogItemConsumerService, UpdatedCatalogItemConsumerService>();
            services.AddSingleton<ICreatedCatalogItemConsumerService, CreatedCatalogItemConsumerService>();

            //ConsumerHosted
            services.AddHostedService<CreatedCatalogItemConsumerHostedService>();
            services.AddHostedService<CreatedCatalogItemConsumerHostedService>();
            services.AddHostedService<CreatedCatalogItemConsumerHostedService>();

            return services;
        }
    }
}
