using Amazon.SQS.Model;
using AspNetCoreMessageBusWorker.Domain.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AspNetCoreMessageBusWorker.Worker.Workers;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IAmazonSQS _sqsClient;
    private readonly IConfiguration _configuration;

    public Worker(
        ILogger<Worker> logger,
        IAmazonSQS sqsClient,
        IConfiguration configuration)
    {
        _logger = logger;
        _sqsClient = sqsClient;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var messages = await PullMessages();

            if (messages.Any())
                messages.ToList().ForEach(message => Process(message));
        }
    }

    private async Task<IEnumerable<Message>> PullMessages()
    {
        var pullingResponse = await _sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest
        {
            QueueUrl = _configuration["AWS:SQS:QueueUrl"],
            MaxNumberOfMessages = int.TryParse(_configuration["AWS:SQS:MaxNumberOfMessages"], out int result) ? result : 1
        });

        return pullingResponse.Messages;
    }

    private void Process(Message message)
    {
        var person = JsonSerializer.Deserialize<Person>(message.Body);

        _logger.LogInformation("Person Processed", person);
    }
}
