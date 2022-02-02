using Amazon.SQS.Model;
using AspNetCoreMessageBusWorker.Domain.Events;
using System.Text.Json;

namespace AspNetCoreMessageBusWorker.Domain.Extensions;

public static class MessageExtensions
{
    public static (string type, OnboardingEvent @event) GetMessageTypeAndEvent(this Message message)
    {
        var @event = JsonSerializer.Deserialize<JsonElement>(message.Body);

        var onboardingEvent = @event.Deserialize<OnboardingEvent>();

        var type = @event.GetProperty("Type").ToString();

        return (type, onboardingEvent);
    }
}
