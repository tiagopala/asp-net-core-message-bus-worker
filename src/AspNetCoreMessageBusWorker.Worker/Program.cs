using AspNetCoreMessageBusWorker.Worker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;

        var builder = new ConfigurationBuilder()
            .SetBasePath(hostContext.HostingEnvironment.ContentRootPath)
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", true)
            .AddEnvironmentVariables();

        services.AddServices(configuration);
    })
    .Build();

await host.RunAsync();