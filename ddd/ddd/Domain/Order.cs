namespace ddd.Domain
{
    public class Order
    {
        private readonly List<IDomainEvent> _domainEvents = new();

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
        public Guid Id { get; private set; }
        public CustomerName CustomerName { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Order()
        {
            
        }
        public Order(CustomerName customerName)
        {
            Id = Guid.NewGuid();
            CustomerName = customerName;
            CreatedAt = DateTime.UtcNow;
            // Dodajemy event po utworzeniu zamówienia
            AddDomainEvent(new OrderCreatedEvent(this));
        }
        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
