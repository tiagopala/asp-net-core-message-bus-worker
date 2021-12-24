using AspNetCoreMessageBusWorker.Worker.Workers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;

        var builder = new ConfigurationBuilder()
            .SetBasePath(hostContext.HostingEnvironment.ContentRootPath)
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", true)
            .AddEnvironmentVariables();

        var options = configuration.GetAWSOptions();
        services.AddAWSService<IAmazonSQS>(options);
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
