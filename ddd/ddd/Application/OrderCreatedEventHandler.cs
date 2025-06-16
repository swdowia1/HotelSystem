using ddd.Domain;
using MediatR;

namespace ddd.Application
{
    public class OrderCreatedEventHandler : INotificationHandler<OrderCreatedEvent>
    {
        public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
        {
            // Tutaj logika, która ma się wykonać po utworzeniu zamówienia
            Console.WriteLine($"Order created with Id: {notification.Order.Id}");
            return Task.CompletedTask;
        }
    }
}
