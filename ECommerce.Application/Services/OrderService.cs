    using ECommerce.Application.Interfaces;
    using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Repositories;
using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace ECommerce.Application.Services
    {
        public class OrderService : IOrderService
        {
            private readonly OrderRepository _orderRepository;

            public OrderService(OrderRepository orderRepository)
            {
                _orderRepository = orderRepository;
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
                // Add business logic here if needed (validation, calculations, etc.)
                order.OrderDate = DateTime.UtcNow;
                await _orderRepository.AddOrderAsync(order);
            }

            public async Task UpdateOrderAsync(Order order)
            {
                // Add business logic here if needed (validation, status checks, etc.)
                var existingOrder = await _orderRepository.GetOrderByIdAsync(order.Id);
                if (existingOrder == null)
                {
                    throw new InvalidOperationException($"Order with ID {order.Id} not found.");
                }

                await _orderRepository.UpdateOrderAsync(order);
            }

            public async Task DeleteOrderAsync(int id)
            {
                var existingOrder = await _orderRepository.GetOrderByIdAsync(id);
                if (existingOrder == null)
                {
                    throw new InvalidOperationException($"Order with ID {id} not found.");
                }

                await _orderRepository.DeleteOrderAsync(id);
            }
        }
    }
