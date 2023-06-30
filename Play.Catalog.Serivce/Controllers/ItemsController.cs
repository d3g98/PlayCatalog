using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Contracts;
using Play.Catalog.Serivce.Dtos;
using Play.Catalog.Serivce.Entities;
using Play.Catalog.Serivce.RabbitMQ;
using Play.Common;

namespace Play.Catalog.Serivce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<Item> _itemsRepository;
        private readonly IRabitMQProducer _rabitMQProducer;
        public ItemsController(IRepository<Item> itemsRepository, IRabitMQProducer rabitMQProducer)
        {
            _itemsRepository = itemsRepository;
            _rabitMQProducer = rabitMQProducer;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAsync()
        {
            var items = (await _itemsRepository.GetAllAsync()).Select(i => i.AsDto());
            return items;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {
            var item = await _itemsRepository.GetAsync(id);
            if (item == null) return NotFound();
            return item.AsDto();
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(CreateItemDto createItemDto)
        {
            Item item = new Item()
            {
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                Price = createItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
            await _itemsRepository.CreateAsync(item);

            _rabitMQProducer.SendMessage(new CatalogItemCreated(item.Id, item.Name, item.Description));

            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(Guid id, UpdateItemDto updateItemDto)
        {
            var existingItem = await _itemsRepository.GetAsync(id);

            if (existingItem == null) return NotFound();

            existingItem.Name = updateItemDto.Name;
            existingItem.Description = updateItemDto.Description;
            existingItem.Price = updateItemDto.Price;

            await _itemsRepository.UpdateAsync(existingItem);

            _rabitMQProducer.SendMessage(new CatalogItemUpdated(existingItem.Id, existingItem.Name, existingItem.Description));

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingItem = await _itemsRepository.GetAsync(id);

            if (existingItem == null) return NotFound();

            await _itemsRepository.DeleteAsync(id);

            _rabitMQProducer.SendMessage(new CatalogItemDeleted(existingItem.Id));

            return NoContent();
        }
    }
}
