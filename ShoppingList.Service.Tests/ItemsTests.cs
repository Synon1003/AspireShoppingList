using NUnit.Framework.Internal;
using RestSharp;
using ShoppingList.service.Tests.Dtos;
using ShoppingList.Service.Tests.Helpers;

namespace ShoppingList.Service.Tests;

public class ItemsTests: TestsBase
{
    [Test, Description("Test get all items")]
    public async Task TestGetItems()
    {
        // Arrange
        TestContext.Progress.WriteLine($"## Starting {TestContext.CurrentContext.Test.Name} ##");

        // Act
        RestResponse getResponse = await _serviceClient.GetItemsAsync();
        List<ItemDto>? items = Deserializers.DeserializeJsonList<ItemDto>(getResponse.Content!);

        // Assert
        Assert.That(getResponse.IsSuccessful, Is.True);
        Assert.That(items, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(items, Has.Count.GreaterThanOrEqualTo(3));
            Assert.That(items[0].Name, Is.EqualTo("Apples"));
            Assert.That(items[1].Name, Is.EqualTo("Bananas"));
            Assert.That(items[2].Name, Is.EqualTo("Carrots"));

            Assert.That(items[0].Description, Is.EqualTo("Fresh red apples"));
            Assert.That(items[0].Price, Is.EqualTo(300));
            Assert.That(items[0].Status, Is.EqualTo("NotPurchased"));
        }
    }

    [Test, Description("Test get a single item by Id")]
    public async Task TestGetItem()
    {
        // Arrange
        TestContext.Progress.WriteLine($"## Starting {TestContext.CurrentContext.Test.Name} ##");
        RestResponse itemsResponse = await _serviceClient.GetItemsAsync();
        List<ItemDto>? allItems = Deserializers.DeserializeJsonList<ItemDto>(itemsResponse.Content!);
        string itemId = allItems![0].Id.ToString();

        // Act
        RestResponse getResponse = await _serviceClient.GetItemAsync(itemId);
        ItemDto? item = Deserializers.DeserializeJson<ItemDto>(getResponse.Content!);

        // Assert
        Assert.That(getResponse.IsSuccessful, Is.True);
        Assert.That(item, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(item.Id.ToString(), Is.EqualTo(itemId));
            Assert.That(item.Name, Is.EqualTo(allItems[0].Name));
            Assert.That(item.Description, Is.EqualTo(allItems[0].Description));
            Assert.That(item.Price, Is.EqualTo(allItems[0].Price));
            Assert.That(item.Status, Is.EqualTo(allItems[0].Status));
            Assert.That(item.UpdatedAt, Is.EqualTo(allItems[0].UpdatedAt));
        }
    }

    [Test, Description("Add a new item")]
    public async Task TestAddItem()
    {
        // Arrange
        TestContext.Progress.WriteLine($"## Starting {TestContext.CurrentContext.Test.Name} ##");
        var newItem = new CreateItemDto(Name: "Oranges", Description: "Juicy oranges", Price: 250);

        // Act
        RestResponse addResponse = await _serviceClient.PostItemAsync(newItem.ToDictionary());
        ItemDto? createdItem = Deserializers.DeserializeJson<ItemDto>(addResponse.Content!);

        RestResponse getResponse = await _serviceClient.GetItemAsync(createdItem!.Id.ToString());
        ItemDto? existingItem = Deserializers.DeserializeJson<ItemDto>(getResponse.Content!);

        // Assert
        Assert.That(addResponse.IsSuccessful, Is.True);
        Assert.That(existingItem, Is.Not.Null);
        Assert.That(existingItem, Is.EqualTo(createdItem));
        using (Assert.EnterMultipleScope())
        {
            Assert.That(existingItem!.Name, Is.EqualTo(newItem.Name));
            Assert.That(existingItem.Description, Is.EqualTo(newItem.Description));
            Assert.That(existingItem.Price, Is.EqualTo(newItem.Price));
            Assert.That(existingItem.Status, Is.EqualTo("NotPurchased"));
            Assert.That(existingItem.UpdatedAt, Is.EqualTo(existingItem!.UpdatedAt));
        }
    }

    [Test, Description("Toggle item status")]
    public async Task TestToggleItem()
    {
        // Arrange
        TestContext.Progress.WriteLine($"## Starting {TestContext.CurrentContext.Test.Name} ##");
        RestResponse itemsResponse = await _serviceClient.GetItemsAsync();
        List<ItemDto>? allItems = Deserializers.DeserializeJsonList<ItemDto>(itemsResponse.Content!);
        string itemId = allItems![0].Id.ToString();

        // Act
        RestResponse patchFirstResponse = await _serviceClient.PatchItemAsync(itemId);
        RestResponse getResponse = await _serviceClient.GetItemAsync(itemId);
        ItemDto? patchedOnceItem = Deserializers.DeserializeJson<ItemDto>(getResponse.Content!);

        RestResponse patchSecondResponse = await _serviceClient.PatchItemAsync(itemId);
        getResponse = await _serviceClient.GetItemAsync(itemId);
        ItemDto? patchedTwiceItem = Deserializers.DeserializeJson<ItemDto>(getResponse.Content!);

        // Assert
        Assert.That(patchFirstResponse.IsSuccessful, Is.True);
        Assert.That(patchSecondResponse.IsSuccessful, Is.True);
        Assert.That(patchedOnceItem, Is.Not.Null);
        Assert.That(patchedTwiceItem, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(patchedOnceItem.Status, Is.EqualTo("Purchased"));
            Assert.That(patchedOnceItem.UpdatedAt, Is.GreaterThan(allItems[0].UpdatedAt));
            Assert.That(patchedTwiceItem.Status, Is.EqualTo("NotPurchased"));
            Assert.That(patchedTwiceItem.UpdatedAt, Is.GreaterThan(patchedOnceItem.UpdatedAt));
        }
    }

    [Test, Description("Delete a single item by Id")]
    public async Task TestDeleteItem()
    {
        // Arrange
        TestContext.Progress.WriteLine($"## Starting {TestContext.CurrentContext.Test.Name} ##");
        RestResponse itemsResponse = await _serviceClient.GetItemsAsync();
        List<ItemDto>? allItems = Deserializers.DeserializeJsonList<ItemDto>(itemsResponse.Content!);
        string itemId = allItems![0].Id.ToString();

        // Act
        await _serviceClient.DeleteItemAsync(itemId);
        RestResponse getResponse = await _serviceClient.GetItemAsync(itemId);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(getResponse.IsSuccessful, Is.False);
            Assert.That(getResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));
        }
    }
}
