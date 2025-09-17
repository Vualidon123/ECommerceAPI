using ECommerceAPI.Models;
using ECommerceAPI.Repositories;

namespace ECommerceAPI.Services
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly OrderDetailRepository _orderDetailRepository;
        public OrderService(OrderRepository orderRepository, OrderDetailRepository orderDetailRepository)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllOrdersAsync();
        }
        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _orderRepository.GetOrderByIdAsync(id);
        }

        public async Task AddOrderAsync(Order order)
        {
            await _orderRepository.AddOrderAsync(order);
            foreach (var detail in order.OrderDetails)
            {
                detail.id = order.id; // Ensure the OrderId is set
                await _orderDetailRepository.UpdateOrderDetailAsync(detail);
            }
        }
        public async Task UpdateOrderAsync(Order order)
        {
            await _orderRepository.UpdateOrderAsync(order);
            foreach (var detail in order.OrderDetails)
            {
                await _orderDetailRepository.UpdateOrderDetailAsync(detail);
            }
        }
    }
}
