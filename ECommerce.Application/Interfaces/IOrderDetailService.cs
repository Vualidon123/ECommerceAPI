using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces
{
    public interface IOrderDetailService
    {
        Task<IEnumerable< OrderDetails>> GetAllOrderDetailsAsync();
        Task<OrderDetails?> GetOrderDetailByIdAsync(int id);
        Task AddOrderDetailAsync(OrderDetails orderDetail);
        Task UpdateOrderDetailAsync(OrderDetails orderDetail);
        Task DeleteOrderDetailAsync(int id);
    }
}
