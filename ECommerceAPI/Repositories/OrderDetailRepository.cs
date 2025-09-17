using ECommerceAPI.Data;
using ECommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Repositories
{
    public class OrderDetailRepository
    {
        private readonly ECommerceDbContext _context;
        public OrderDetailRepository(ECommerceDbContext context) {
            _context = context;
        }
        public async Task<IEnumerable<OrderDetails>> GetAllOrderDetailsAsync()
        {
            return await _context.OrderDetails.ToListAsync();
        }
        public async Task<OrderDetails> GetOrderDetailByIdAsync(int id)
        {
            return await _context.OrderDetails.FindAsync(id);
        }
        public async Task AddOrderDetailAsync(OrderDetails orderDetail)
        {
             _context.OrderDetails.Add(orderDetail);
            _context.SaveChanges();
        }
        public async Task UpdateOrderDetailAsync(OrderDetails orderDetail)
        {
            _context.Entry(orderDetail).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public async Task DeleteOrderDetailAsync(int id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail != null)
            {
                _context.OrderDetails.Remove(orderDetail);
                _context.SaveChanges();
            }
        }
    }
}
