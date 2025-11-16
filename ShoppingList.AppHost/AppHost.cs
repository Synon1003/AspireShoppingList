var builder = DistributedApplication.CreateBuilder(args);

var mongo = builder.AddMongoDB("mongo", 59437)
    .WithDataBindMount(@"C:\Users\User\Documents\Volumes\MongoDB\Data");
var mongodb = mongo.AddDatabase("mongodb");

var apiService = builder.AddProject<Projects.ShoppingList_Service>("apiservice")
    .WaitFor(mongodb)
    .WithReference(mongodb);

var webApp = builder.AddViteApp("webapp", "../ShoppingList.Client", "pnpm")
    .WithPnpmPackageInstallation()
    .WaitFor(apiService);

builder.Build().Run();
