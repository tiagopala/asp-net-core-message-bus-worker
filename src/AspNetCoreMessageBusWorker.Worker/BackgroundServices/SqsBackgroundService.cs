using Amazon.SQS.Model;
using AspNetCoreMessageBusWorker.Domain.Commands;
using AspNetCoreMessageBusWorker.Domain.Events;
using AspNetCoreMessageBusWorker.Domain.Extensions;

namespace AspNetCoreMessageBusWorker.Worker.BackgroundServices;

public class SqsBackgroundService : BaseBackgroundService
{
    private readonly ILogger<SqsBackgroundService> _logger;
    private readonly IMediator _mediatR;

    public SqsBackgroundService(
        ILogger<SqsBackgroundService> logger,
        IAmazonSQS sqsClient,
        IConfiguration configuration,
        IMediator mediatR) : base(logger, sqsClient, configuration)
    {
        _logger = logger;
        _mediatR = mediatR;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var messages = await PullForMessages();

            if (messages.Any())
            {
                foreach (var message in messages.ToList())
                {
                    await Process(message);
                }
            }
        }
    }

    private async Task Process(Message message)
    {
        try
        {
            var (type, @event) = message.GetMessageTypeAndEvent();

            var command = GetCommandFromMessageType(type, @event);

            await _mediatR.Send(command);

            await RemoveProcessedMessage(message);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }
    }

    private static Command GetCommandFromMessageType(string type, OnboardingEvent @event)
    {
        return type switch
        {
            "CreateCustomer" => @event.ToCreateCustomerCommand(),
            _ => throw new ArgumentOutOfRangeException($"Invalid Message Type: {type}."),
        };
    }
}
