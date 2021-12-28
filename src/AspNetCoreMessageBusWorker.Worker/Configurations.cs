using AspNetCoreMessageBusWorker.Domain.Commands;
using AspNetCoreMessageBusWorker.Worker.BackgroundServices;

namespace AspNetCoreMessageBusWorker.Worker;

public static class Configurations
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddMediatR(typeof(SqsBackgroundService), typeof(CreateCustomerCommand));

        var options = configuration.GetAWSOptions();
        services.AddAWSService<IAmazonSQS>(options);
        services.AddHostedService<SqsBackgroundService>();

        return services;
    }
}
