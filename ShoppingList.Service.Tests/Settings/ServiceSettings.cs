using Microsoft.Extensions.Configuration;

namespace ShoppingList.Service.Tests.Settings;

public class ServiceSettings
{
    public string BaseUrl { get; set; } = string.Empty;
    public string ItemsPath { get; set; } = string.Empty;

    public static ServiceSettings LoadFromFile(string file = "servicesettings.json")
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(file, optional: false, reloadOnChange: true)
            .Build();

        return configuration.GetSection("ServiceSettings").Get<ServiceSettings>() ?? new ServiceSettings();
    }
}