using Amazon.SQS.Model;
using System.Net;

namespace AspNetCoreMessageBusWorker.Worker.BackgroundServices;

public abstract class BaseBackgroundService : BackgroundService
{
    private readonly ILogger<BaseBackgroundService> _logger;
    private readonly IAmazonSQS _sqsClient;
    private readonly IConfiguration _configuration;

    public BaseBackgroundService(
        ILogger<BaseBackgroundService> logger,
        IAmazonSQS sqsClient,
        IConfiguration configuration)
    {
        _logger = logger;
        _sqsClient = sqsClient;
        _configuration = configuration;
    }

    protected async Task<IEnumerable<Message>> PullForMessages()
    {
        var pullingResponse = await _sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest
        {
            QueueUrl = _configuration["AWS:SQS:QueueUrl"],
            MaxNumberOfMessages = int.TryParse(_configuration["AWS:SQS:MaxNumberOfMessages"], out int result) ? result : 1
        });

        return pullingResponse.Messages;
    }

    protected async Task RemoveProcessedMessage(Message message)
    {
        var deleteMessageResponse = await _sqsClient.DeleteMessageAsync(new DeleteMessageRequest
        {
            QueueUrl = _configuration["AWS:SQS:QueueUrl"],
            ReceiptHandle = message.ReceiptHandle
        });

        if (deleteMessageResponse.HttpStatusCode is not HttpStatusCode.OK)
            throw new AmazonSQSException($"Message {message.MessageId} could not be removed from the queue. ReceiptHandle: {message.ReceiptHandle}.");
    }
}
