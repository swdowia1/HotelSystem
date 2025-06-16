namespace ddd.Domain
{
    public class OrderCreatedEvent : DomainEventBase
    {
        public Order Order { get; }

        public OrderCreatedEvent(Order order)
        {
            Order = order;
        }
    }

}