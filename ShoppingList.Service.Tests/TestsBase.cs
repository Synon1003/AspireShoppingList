using ShoppingList.service.Tests.Dtos;
using ShoppingList.Service.Tests.Client;

namespace ShoppingList.Service.Tests;

public class TestsBase
{
    protected ServiceClient _serviceClient;
    protected List<CreateItemDto> _items = new()
    {
        new CreateItemDto(Name: "Apples", Description: "Fresh red apples", Price: 300),
        new CreateItemDto(Name: "Bananas", Description: "Ripe yellow bananas", Price: 200),
        new CreateItemDto(Name: "Carrots", Description: "Organic carrots", Price: 150)
    };

    [OneTimeSetUp] public void GlobalSetup() => _serviceClient = new ServiceClient();
    [SetUp] public async Task Setup() =>
        await _serviceClient.InitializeItemsAsync([.. _items.Select(dto => dto.ToDictionary())]);
    [TearDown] public async Task TearDown() => await _serviceClient.ClearItemsAsync();
}
