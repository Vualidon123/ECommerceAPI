using ECommerceAPI.Models;
using ECommerceAPI.Repositories;

namespace ECommerceAPI.Services
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly OrderDetailRepository _orderDetailRepository;
        private readonly ProductRepository _productRepository;
        public OrderService(OrderRepository orderRepository, OrderDetailRepository orderDetailRepository, ProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _productRepository = productRepository;
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
            if (order == null) throw new ArgumentNullException(nameof(order));

            // Ensure server controls identity columns
            order.id = 0;

            order.totalAmount = 0;

            if (order.OrderDetails != null && order.OrderDetails.Count > 0)
            {
                foreach (var detail in order.OrderDetails)
                {
                    // Ensure server controls identity columns
                    detail.id = 0;
                    var product = await _productRepository.GetProductByIdAsync(detail.productId);
                    if (product == null) throw new InvalidOperationException($"Product {detail.productId} not found.");
                    product.stock -= detail.quantity;
                    order.totalAmount += (product.price*detail.quantity);
                }
            }

            await _orderRepository.AddOrderAsync(order);

            if (order.OrderDetails != null && order.OrderDetails.Count > 0)
            {
                foreach (var detail in order.OrderDetails)
                {
                    // Set FK after order is saved so it has a generated id
                    detail.orderId = order.id;
                    await _orderDetailRepository.AddOrderDetailAsync(detail);
                }
            }
        }
        public async Task UpdateOrderAsync(Order order)
        {
            await _orderRepository.UpdateOrderAsync(order);
            if (order.OrderDetails != null && order.OrderDetails.Count > 0)
            {
                foreach (var detail in order.OrderDetails)
                {
                    detail.orderId = order.id;
                    await _orderDetailRepository.UpdateOrderDetailAsync(detail);
                }
            }
        }
    }
}