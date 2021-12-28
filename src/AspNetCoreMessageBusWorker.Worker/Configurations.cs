using AspNetCoreMessageBusWorker.Domain.Commands;
using AspNetCoreMessageBusWorker.Worker.Workers;

namespace AspNetCoreMessageBusWorker.Worker;

public static class Configurations
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddMediatR(typeof(BaseWorker), typeof(CreateCustomerCommand));

        var options = configuration.GetAWSOptions();
        services.AddAWSService<IAmazonSQS>(options);
        services.AddHostedService<BaseWorker>();

        return services;
    }
}
