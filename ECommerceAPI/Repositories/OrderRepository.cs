using ECommerceAPI.Data;
using ECommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Repositories
{
    public class OrderRepository
    {
        private readonly ECommerceDbContext _context;
        public OrderRepository(ECommerceDbContext context) {
            _context = context;
        }
        public async Task<IEnumerable<Order>> GetAllOrdersAsync() {
            return await _context.Orders.ToListAsync();
        }
        public async Task<Order?> GetOrderByIdAsync(int id) {
            return await _context.Orders.FindAsync(id);
        }
        public async Task AddOrderAsync(Order order) {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }
        public async Task UpdateOrderAsync(Order order) {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }
        public async Task DeleteOrderAsync(int id) {
            var order = await _context.Orders.FindAsync(id);
            if (order != null) {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
        }
    }
}
