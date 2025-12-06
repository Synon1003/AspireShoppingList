namespace ShoppingList.service.Tests.Dtos;

public record ItemDto(
    Guid Id,
    string Name,
    string Description,
    int Price,
    string Status,
    DateTimeOffset UpdatedAt
);

public record CreateItemDto(
    string Name,
    string Description,
    int Price
)
{
    public Dictionary<string, object> ToDictionary()
    {
        return new Dictionary<string, object>
        {
            { "name", Name },
            { "description", Description },
            { "price", Price }
        };
    }
}
