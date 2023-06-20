using Play.Catalog.Serivce.Entities;

namespace Play.Catalog.Serivce.Repositories
{
    public interface IItemsRepository
    {
        Task<IReadOnlyCollection<Item>> GetAllAsync();

        Task<Item> GetAsync(Guid id);

        Task CreateAsync(Item entity);

        Task UpdateAsync(Guid id, Item entity);

        Task DeleteAsync(Guid id);
    }
}
