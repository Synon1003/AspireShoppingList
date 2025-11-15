namespace ShoppingList.Core.Dtos;

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
);
