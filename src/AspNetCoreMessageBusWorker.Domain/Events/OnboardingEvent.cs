using AspNetCoreMessageBusWorker.Domain.Commands;
using AspNetCoreMessageBusWorker.Domain.Models;
using System.Text.Json;

namespace AspNetCoreMessageBusWorker.Domain.Events;

public class OnboardingEvent
{
    public string Type { get; set; }
    public JsonElement Message { get; set; }

    public CreateCustomerCommand ToCreateCustomerCommand()
    {
        var customer = Message.GetProperty("CustomerData").Deserialize<Customer>();
        return new CreateCustomerCommand(customer.Id, customer.Name, customer.LastName, customer.Email, customer.Cpf);
    }
}
