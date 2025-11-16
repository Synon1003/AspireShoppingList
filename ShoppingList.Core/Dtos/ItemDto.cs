using System.ComponentModel.DataAnnotations;

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
    [MaxLength(50)] string Name,
    [MaxLength(200)] string Description,
    [Range(0, 100000)] int Price
);
