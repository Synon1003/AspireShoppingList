using ShoppingList.Core.Domain.Entities;
using ShoppingList.Core.Dtos;

namespace ShoppingList.Core.Mappings;

public static class ItemExtensions
{
    public static ItemDto ToDto(this Item item)
    {

        return new ItemDto(
            item.Id,
            item.Name,
            item.Description,
            item.Price,
            item.Status.ToString(),
            item.UpdatedAt
        );
    }
}
