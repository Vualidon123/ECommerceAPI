using ECommerceAPI.Data;
using ECommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Repositories
{
    public class OrderDetailRepository
    {
        private readonly ECommerceDbContext _context;
        public OrderDetailRepository(ECommerceDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<OrderDetails>> GetAllOrderDetailsAsync()
        {
            return await _context.OrderDetails.AsNoTracking().ToListAsync();
        }
        public async Task<OrderDetails?> GetOrderDetailByIdAsync(int id)
        {
            return await _context.OrderDetails.AsNoTracking().FirstOrDefaultAsync(od => od.id == id);
        }
        public async Task AddOrderDetailAsync(OrderDetails orderDetail)
        {
            orderDetail.id = 0;
            await _context.OrderDetails.AddAsync(orderDetail);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateOrderDetailAsync(OrderDetails orderDetail)
        {
            var tracked = _context.ChangeTracker.Entries<OrderDetails>()
                .FirstOrDefault(e => e.Entity.id == orderDetail.id);
            if (tracked != null) {
                _context.Entry(tracked.Entity).State = EntityState.Detached;
            }

            _context.Entry(orderDetail).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteOrderDetailAsync(int id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail != null)
            {
                _context.OrderDetails.Remove(orderDetail);
                await _context.SaveChangesAsync();
            }
        }
    }
}