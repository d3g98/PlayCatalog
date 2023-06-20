using Play.Catalog.Serivce.Dtos;
using Play.Catalog.Serivce.Entities;

namespace Play.Catalog.Serivce
{
    public static class Extensions
    {
        public static ItemDto AsDto(this Item item)
        {
            return new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
        }
    }
}
