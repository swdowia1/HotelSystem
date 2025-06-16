using ddd.VM;
using MediatR;

namespace ddd.Application
{
    public class ListOrdersQuery : IRequest<List<OrderDto>> { }
}
