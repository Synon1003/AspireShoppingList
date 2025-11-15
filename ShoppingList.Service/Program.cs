using Microsoft.AspNetCore.Mvc;
using ShoppingList.Core.Domain.Entities;
using ShoppingList.Core.Domain.RepositoryContracts;
using ShoppingList.Core.Mappings;
using ShoppingList.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddProblemDetails();

builder.AddMongoDBClient("mongodb");
builder.Services.AddItemRepository("items");

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var itemRepository = scope.ServiceProvider.GetRequiredService<IRepository<Item>>();

    var collection = await itemRepository.GetAllAsync();

    // Check if data exists
    if (collection.Count == 0)
    {
        var items = new List<Item>();

        foreach (var index in Enumerable.Range(1, 5))
        {
            await itemRepository.InsertAsync(new Item
            {
                Id = Guid.NewGuid(),
                Name = $"Item{index}",
                Description = $"Description for item {index}",
                Price = Random.Shared.Next(0, 55),
                Status = ItemStatus.NotPurchased
            });
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.MapGet("/items", async ([FromServices] IRepository<Item> repository) =>
{
    var items = await repository.GetAllAsync();
    return items.Select(i => i.ToDto()).ToArray();
}).WithName("GetItems");

app.MapDefaultEndpoints();

app.Run();
