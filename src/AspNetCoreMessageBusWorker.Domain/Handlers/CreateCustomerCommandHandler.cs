using AspNetCoreMessageBusWorker.Domain.Commands;
using MediatR;

namespace AspNetCoreMessageBusWorker.Domain.Handlers
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand>
    {
        public async Task<Unit> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            // TODO Implementar lógica para criação do cliente
            throw new NotImplementedException();
        }
    }
}
