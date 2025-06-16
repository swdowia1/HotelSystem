using MediatR;

namespace ddd.Application
{
    public class CreateOrderCommand : IRequest<Unit>
    {
        public string CustomerName { get; init; } = string.Empty;
    }
}
