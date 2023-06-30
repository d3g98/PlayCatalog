using Microsoft.AspNetCore.Mvc;
using Play.Common;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.Dtos;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<InventoryItem> _itemRepository;
        private readonly CatalogClient _catalogClient;
        public ItemsController(IRepository<InventoryItem> itemRepository, CatalogClient catalogClient)
        {
            _itemRepository = itemRepository;
            _catalogClient = catalogClient;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userId)
        {
            if (userId == Guid.Empty) return BadRequest();
            var catalogItems = await _catalogClient.GetCatalogItemsAsync();
            var inventoryItemEntities = await _itemRepository.GetAllAsync(item => item.UserId == userId);

            //var items = inventoryItemEntities.Select(it => {
            //    var catalogItem = catalogItems.Single(catalogItem => catalogItem.Id == it.CatalogItemId);
            //    return it.AsDto(catalogItem.Name, catalogItem.Description);
            //});

            List<InventoryItemDto> items = new List<InventoryItemDto>();
            foreach (var it in inventoryItemEntities)
            {
                var catalogItem = catalogItems.FirstOrDefault(catalogItem => catalogItem.Id == it.CatalogItemId);
                if (catalogItem != null)
                {
                    items.Add(it.AsDto(catalogItem.Name, catalogItem.Description));
                }
            }
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(GrantItemsDto grantItemsDto)
        {
            var inventoryItem = await _itemRepository.GetAsync(item => item.UserId == grantItemsDto.UserId
            && item.CatalogItemId == grantItemsDto.CatalogItemId);

            if (inventoryItem == null)
            {
                inventoryItem = new InventoryItem()
                {
                    CatalogItemId = grantItemsDto.CatalogItemId,
                    UserId = grantItemsDto.UserId,
                    Quantity = grantItemsDto.Quantity,
                    AcquiredDate = DateTimeOffset.UtcNow
                };

                await _itemRepository.CreateAsync(inventoryItem);
            }
            else
            {
                inventoryItem.Quantity += grantItemsDto.Quantity;
                await _itemRepository.UpdateAsync(inventoryItem);
            }

            return Ok();
        }
    }
}