using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly ILogger<OrderDetailController> _logger;
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailController(ILogger<OrderDetailController> logger, IOrderDetailService orderDetailService)
        {
            _logger = logger;
            _orderDetailService = orderDetailService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrderDetails()
        {
            try
            {
                var orderDetails = await _orderDetailService.GetAllOrderDetailsAsync();
                return Ok(orderDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all order details");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetailById(int id)
        {
            try
            {
                var orderDetail = await _orderDetailService.GetOrderDetailByIdAsync(id);
                if (orderDetail == null)
                {
                    return NotFound($"Order detail with ID {id} not found");
                }
                return Ok(orderDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting order detail by ID: {OrderDetailId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        //[Authorize]
        //[HttpGet("order/{orderId}")]
        //public async Task<IActionResult> GetOrderDetailsByOrderId(int orderId)
        //{
        //    try
        //    {
        //        var orderDetails = await _orderDetailService.GetOrderDetailsByOrderIdAsync(orderId);
        //        return Ok(orderDetails);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while getting order details by order ID: {OrderId}", orderId);
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrderDetail([FromBody] OrderDetails orderDetail)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _orderDetailService.AddOrderDetailAsync(orderDetail);
                return CreatedAtAction(nameof(GetOrderDetailById), new { id = orderDetail.Id }, orderDetail);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating order detail");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderDetail(int id, [FromBody] OrderDetails orderDetail)
        {
            try
            {
                if (id != orderDetail.Id)
                {
                    return BadRequest("Order detail ID mismatch");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _orderDetailService.UpdateOrderDetailAsync(orderDetail);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating order detail: {OrderDetailId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            try
            {
                await _orderDetailService.DeleteOrderDetailAsync(id);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting order detail: {OrderDetailId}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}