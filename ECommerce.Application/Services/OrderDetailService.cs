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
    public class OrderDetailService : IOrderDetailService
    {
        private readonly OrderDetailRepository _orderDetailRepository;
        private readonly ProductRepository _productRepository;

        public OrderDetailService(OrderDetailRepository orderDetailRepository, ProductRepository productRepository)
        {
            _orderDetailRepository = orderDetailRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<OrderDetails>> GetAllOrderDetailsAsync()
        {
            return await _orderDetailRepository.GetAllOrderDetailsAsync();
        }

        public async Task<OrderDetails?> GetOrderDetailByIdAsync(int id)
        {
            return await _orderDetailRepository.GetOrderDetailByIdAsync(id);
        }

        public  Task<IEnumerable<OrderDetails>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            throw new NotImplementedException()  ; // Not implemented yet
        }

        public async Task AddOrderDetailAsync(OrderDetails orderDetail)
        {
            // Business logic validation
            if (orderDetail.Quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.");
            }

            // Verify product exists and get current price
            var product = await _productRepository.GetProductByIdAsync(orderDetail.ProductId);
            if (product == null)
            {
                throw new InvalidOperationException($"Product with ID {orderDetail.ProductId} not found.");
            }

            // Set unit price from current product price if not already set
            if (orderDetail.UnitPrice <= 0)
            {
                orderDetail.UnitPrice = product.Price;
            }

            // Check stock availability
            if (product.StockQuantity < orderDetail.Quantity)
            {
                throw new InvalidOperationException($"Insufficient stock. Available: {product.StockQuantity}, Requested: {orderDetail.Quantity}");
            }

            await _orderDetailRepository.AddOrderDetailAsync(orderDetail);
        }

        public async Task UpdateOrderDetailAsync(OrderDetails orderDetail)
        {
            // Business logic validation
            if (orderDetail.Quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.");
            }

            // Check if order detail exists
            var existingOrderDetail = await _orderDetailRepository.GetOrderDetailByIdAsync(orderDetail.Id);
            if (existingOrderDetail == null)
            {
                throw new InvalidOperationException($"Order detail with ID {orderDetail.Id} not found.");
            }

            // Verify product still exists
            var product = await _productRepository.GetProductByIdAsync(orderDetail.ProductId);
            if (product == null)
            {
                throw new InvalidOperationException($"Product with ID {orderDetail.ProductId} not found.");
            }

            // Check stock availability for quantity changes
            if (orderDetail.Quantity > existingOrderDetail.Quantity)
            {
                var additionalQuantity = orderDetail.Quantity - existingOrderDetail.Quantity;
                if (product.StockQuantity < additionalQuantity)
                {
                    throw new InvalidOperationException($"Insufficient stock for quantity increase. Available: {product.StockQuantity}, Additional needed: {additionalQuantity}");
                }
            }

            await _orderDetailRepository.UpdateOrderDetailAsync(orderDetail);
        }

        public async Task DeleteOrderDetailAsync(int id)
        {
            var existingOrderDetail = await _orderDetailRepository.GetOrderDetailByIdAsync(id);
            if (existingOrderDetail == null)
            {
                throw new InvalidOperationException($"Order detail with ID {id} not found.");
            }

            await _orderDetailRepository.DeleteOrderDetailAsync(id);
        }
    }
}
