using Amazon.SQS.Model;
using AspNetCoreMessageBusWorker.Domain.Commands;
using AspNetCoreMessageBusWorker.Domain.Events;
using AspNetCoreMessageBusWorker.Domain.Extensions;

namespace AspNetCoreMessageBusWorker.Worker.Workers;

public class BaseWorker : BackgroundService
{
    private readonly ILogger<BaseWorker> _logger;
    private readonly IAmazonSQS _sqsClient;
    private readonly IConfiguration _configuration;
    private readonly IMediator _mediatR;

    public BaseWorker(
        ILogger<BaseWorker> logger,
        IAmazonSQS sqsClient,
        IConfiguration configuration,
        IMediator mediatR)
    {
        _logger = logger;
        _sqsClient = sqsClient;
        _configuration = configuration;
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

    private async Task<IEnumerable<Message>> PullForMessages()
    {
        var pullingResponse = await _sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest
        {
            QueueUrl = _configuration["AWS:SQS:QueueUrl"],
            MaxNumberOfMessages = int.TryParse(_configuration["AWS:SQS:MaxNumberOfMessages"], out int result) ? result : 1
        });

        return pullingResponse.Messages;
    }

    private async Task Process(Message message)
    {
        try
        {
            var (type, @event) = message.GetMessageTypeAndEvent();

            var command = GetCommandFromMessageType(type, @event);

            await _mediatR.Send(command);
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
