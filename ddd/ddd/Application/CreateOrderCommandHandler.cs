using ddd.Domain;
using MediatR;

namespace ddd.Application
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Unit>
    {
        private readonly IOrderRepository _repository;

        public CreateOrderCommandHandler(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var customerName = new CustomerName(request.CustomerName);
            var order = new Order(customerName);
            await _repository.AddAsync(order);
            return Unit.Value;
        }
    }
}
