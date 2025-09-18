using ECommerceAPI.Data;
using ECommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Repositories
{
    public class OrderRepository
    {
        private readonly ECommerceDbContext _context;
        public OrderRepository(ECommerceDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.AsNoTracking().ToListAsync();
        }
        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.id == id);
        }
        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateOrderAsync(Order order)
        {
            var tracked = _context.ChangeTracker.Entries<Order>()
                .FirstOrDefault(e => e.Entity.id == order.id);
            if (tracked != null) {
                _context.Entry(tracked.Entity).State = EntityState.Detached;
            }

            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }
}