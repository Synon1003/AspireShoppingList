using RestSharp;
using ShoppingList.Service.Tests.Settings;

namespace ShoppingList.Service.Tests.Client;
public class ServiceClient
{
    private readonly RestClient _client;
    private readonly ServiceSettings _settings;

    public ServiceClient()
    {
        _settings = ServiceSettings.LoadFromFile();
        _client = new RestClient(_settings.BaseUrl);
    }

    public async Task<RestResponse> GetItemsAsync()
    {
        RestRequest request = new RestRequest(_settings.ItemsPath, Method.Get);
        request.AddHeader("Accept", "application/json");
        TestContext.Progress.WriteLine("Request: " + request.Method + " " + _client.BuildUri(request));

        RestResponse response = await _client.ExecuteAsync(request);
        TestContext.Progress.WriteLine("Response: " + response.Content);

        return response;
    }

    public async Task<RestResponse> GetItemAsync(string itemId)
    {
        RestRequest request = new RestRequest(Path.Combine(_settings.ItemsPath, itemId), Method.Get);
        request.AddHeader("Accept", "application/json");
        TestContext.Progress.WriteLine("Request: " + request.Method + " " + _client.BuildUri(request));

        RestResponse response = await _client.ExecuteAsync(request);
        TestContext.Progress.WriteLine("Response: " + response.Content);

        return response;
    }
    
    public async Task<RestResponse> PostItemAsync(Dictionary<string, object> itemData)
    {
        RestRequest request = new RestRequest(_settings.ItemsPath, Method.Post);
        request.AddHeader("Accept", "application/json");
        request.AddJsonBody(itemData);
        TestContext.Progress.WriteLine("Request: " + request.Method + " " + _client.BuildUri(request));

        RestResponse response = await _client.ExecuteAsync(request);
        TestContext.Progress.WriteLine("Response: " + response.Content);

        return response;
    }

    public async Task<RestResponse> PatchItemAsync(string itemId)
    {
        var request = new RestRequest(Path.Combine(_settings.ItemsPath, itemId), Method.Patch);
        TestContext.Progress.WriteLine("Request: " + request.Method + " " + _client.BuildUri(request));

        RestResponse response = await _client.ExecuteAsync(request);
        TestContext.Progress.WriteLine("Response: " + response.Content);

        return response;
    }

    public async Task<RestResponse> DeleteItemAsync(string itemId)
    {
        var request = new RestRequest(Path.Combine(_settings.ItemsPath, itemId), Method.Delete);
        TestContext.Progress.WriteLine("Request: " + request.Method + " " + _client.BuildUri(request));

        RestResponse response = await _client.ExecuteAsync(request);
        TestContext.Progress.WriteLine("Response: " + response.Content);

        return response;
    }

    public async Task InitializeItemsAsync(List<Dictionary<string, object>> itemsData)
    {
        TestContext.Progress.WriteLine($"## {nameof(InitializeItemsAsync)} ##");
        var request = new RestRequest(Path.Combine(_settings.ItemsPath, "initialize"), Method.Post);
        request.AddJsonBody(itemsData);
        TestContext.Progress.WriteLine("Request: " + request.Method + " " + _client.BuildUri(request));

        RestResponse response = await _client.ExecuteAsync(request);
        TestContext.Progress.WriteLine("Response: " + response.Content);
    }

    public async Task<RestResponse> ClearItemsAsync()
    {
        TestContext.Progress.WriteLine($"## {nameof(ClearItemsAsync)} ##");
        var request = new RestRequest(Path.Combine(_settings.ItemsPath, "clear"), Method.Delete);
        TestContext.Progress.WriteLine("Request: " + request.Method + " " + _client.BuildUri(request));

        RestResponse response = await _client.ExecuteAsync(request);
        TestContext.Progress.WriteLine("Response: " + response.Content);

        return response;
    }
}