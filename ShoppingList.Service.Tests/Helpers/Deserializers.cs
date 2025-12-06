using System.Text.Json;
using System.Text.Json.Serialization;

namespace ShoppingList.Service.Tests.Helpers;

public static class Deserializers
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() },
    };

    public static T? DeserializeJson<T>(string json)
        => JsonSerializer.Deserialize<T>(json, _options);

    public static List<T>? DeserializeJsonList<T>(string json)
        => JsonSerializer.Deserialize<List<T>>(json, _options);
}