using Ecommerce.Core.Models;
using Ecommerce.EF.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
       private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderModelDto orderModel)
        {
            if (orderModel == null)
            {
                return BadRequest("Order model cannot be null.");
            }
            var uid = Request.Cookies["User_id"];
            var createdOrder = await _orderService.CreateOrderAsync(orderModel, uid);
            return Ok(createdOrder);
        }
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(orderId);
                return Ok(order);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Order with ID {orderId} not found.");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllOrders([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var (orders, totalRecords) = await _orderService.GetAllOrdersAsync(pageNumber, pageSize);
            var response = new PagedResponse<OrderModel>(orders, pageNumber, pageSize, totalRecords);
            return Ok(response);
        }
        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrder(int orderId, [FromBody] OrderModelDto orderModel)
        {
            if (orderModel == null)
            {
                return BadRequest("Order model cannot be null.");
            }
            try
            {
                var updatedOrder = await _orderService.UpdateOrderAsync(orderId, orderModel);
                return Ok(updatedOrder);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Order with ID {orderId} not found.");
            }
        }
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var result = await _orderService.DeleteOrderAsync(orderId);
            if (!result)
            {
                return NotFound($"Order with ID {orderId} not found.");
            }
            
            return NoContent();
        }
    }
}
