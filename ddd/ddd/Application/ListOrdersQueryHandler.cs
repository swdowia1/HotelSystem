using ddd.Domain;
using ddd.VM;
using MediatR;
using System;

namespace ddd.Application
{
    public class ListOrdersQueryHandler : IRequestHandler<ListOrdersQuery, List<OrderDto>>
    {
        private readonly IOrderRepository _repository;

        public ListOrdersQueryHandler(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<OrderDto>> Handle(ListOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _repository.GetAllAsync();

            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                CustomerName = o.CustomerName.Value,
                CreatedAt = o.CreatedAt
            }).ToList();
        }
    }
}
