using MongoDB.Driver;
using Play.Catalog.Serivce.Entities;
using SharpCompress.Common;

namespace Play.Catalog.Serivce.Repositories
{
    public class ItemsRepository : IItemsRepository
    {
        private const string collectionName = "items";
        private readonly IMongoCollection<Item> dbCollection;
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

        public ItemsRepository(IMongoDatabase database)
        {
            dbCollection = database.GetCollection<Item>(collectionName);
        }

        public async Task<IReadOnlyCollection<Item>> GetAllAsync()
        {
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        }

        public async Task<Item> GetAsync(Guid id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(e => e.Id, id);
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Item entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            await dbCollection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(Guid id, Item entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            FilterDefinition<Item> filter = filterBuilder.Eq(entity => entity.Id, id);

            await dbCollection.ReplaceOneAsync(filter, entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(entity => entity.Id, id);

            await dbCollection.DeleteOneAsync(filter);
        }
    }
}
