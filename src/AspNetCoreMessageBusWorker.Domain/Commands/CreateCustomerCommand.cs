namespace AspNetCoreMessageBusWorker.Domain.Commands
{
    public class CreateCustomerCommand : Command
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string Cpf { get; private set; }

        public CreateCustomerCommand(Guid id, string name, string lastname, string email, string cpf)
        {
            Id = id;
            Name = name;
            LastName = lastname;
            Email = email;
            Cpf = cpf;
        }
    }
}
