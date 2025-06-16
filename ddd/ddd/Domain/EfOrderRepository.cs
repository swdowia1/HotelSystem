using Microsoft.EntityFrameworkCore;

namespace ddd.Domain
{
    class EfOrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public EfOrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task<Order> GetByIdAsync(Guid id)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.Id == id)!;
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders.ToListAsync();
        }
    }
}
