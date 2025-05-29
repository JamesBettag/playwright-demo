var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.PlaywrightDemo_ApiService>("apiservice");

builder.AddProject<Projects.PlaywrightDemo_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
