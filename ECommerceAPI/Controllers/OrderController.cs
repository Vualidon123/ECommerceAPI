using ECommerceAPI.Models;
using ECommerceAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            await _orderService.AddOrderAsync(order);
            return CreatedAtAction(nameof(GetOrderById), new { id = order.id }, order);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] Models.Order order)
        {
            if (id != order.id)
            {
                return BadRequest();
            }
            await _orderService.UpdateOrderAsync(order);
            return NoContent();
        }
        /* [HttpDelete("{id}")]
         public async Task<IActionResult> DeleteOrder(int id) {
             await _orderService.DeleteOrderAsync(id);
             return NoContent();
         }*/
    }
}