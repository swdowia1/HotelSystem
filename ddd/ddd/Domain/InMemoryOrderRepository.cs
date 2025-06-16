namespace ddd.Domain
{
    public class InMemoryOrderRepository : IOrderRepository
    {
        private readonly List<Order> _orders = new();

        public Task AddAsync(Order order)
        {
            _orders.Add(order);
            return Task.CompletedTask;
        }

        public Task<Order> GetByIdAsync(Guid id)
        {
            return Task.FromResult(_orders.FirstOrDefault(o => o.Id == id));
        }
    }
}
