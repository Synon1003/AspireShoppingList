using Microsoft.AspNetCore.HttpLogging;
using MongoDB.Driver;
using ShoppingList.Core.Domain.Entities;
using ShoppingList.Core.Domain.RepositoryContracts;
using ShoppingList.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddProblemDetails();
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.RequestProperties
        | HttpLoggingFields.ResponsePropertiesAndHeaders;
});

builder.AddMongoDBClient("mongodb");
builder.Services.AddItemRepository("items");
builder.Services.AddScoped<IRepository<Item>>(
    sp => new MongoRepository<Item>(sp.GetRequiredService<IMongoDatabase>(), "items"));
builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        c.RoutePrefix = "swagger";
    });

    app.UseCors(b => b.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
}

app.UseHttpLogging();
app.UseHttpsRedirection();

app.MapControllers();
app.MapDefaultEndpoints();

app.Run();
