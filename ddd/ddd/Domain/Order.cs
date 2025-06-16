namespace ddd.Domain
{
    public class Order
    {
        public Guid Id { get; private set; }
        public CustomerName CustomerName { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Order(CustomerName customerName)
        {
            Id = Guid.NewGuid();
            CustomerName = customerName;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
