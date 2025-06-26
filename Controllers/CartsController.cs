using Ecommerce.Core.Models;
using Ecommerce.EF.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
       private readonly ICartsService _cartsService;
        public CartsController(ICartsService cartsService)
        {
            _cartsService = cartsService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCarts([FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var (carts, totalRecords) = await _cartsService.GetAllCartsAsync(pageSize, pageNumber);
            var response = new PagedResponse<CartsModel>(carts, pageNumber, pageSize, totalRecords);
            return Ok(response);
        }
        [HttpGet("{CartId}")]
        public async Task<IActionResult> GetCartById (int CartId)
        {
            var cart = await _cartsService.GetCartByIdAsync(CartId);
            return Ok(cart);
        }
        [HttpPost]
        public async Task<IActionResult> CreatCart ([FromBody] CartsModelDto cartModel)
        {
            if (cartModel == null)
            {
                return BadRequest("Cart model cannot be null.");
            }
            var uid = Request.Cookies["User_id"];
            var createdCart = await _cartsService.CreateCartsAsync(cartModel, uid);
            return Ok(createdCart);
        }
        [HttpPut("{CartId}")]
        public async Task<IActionResult> UpdateCart(int CartId, [FromBody] CartsModelDto cartModel)
        {
            if (cartModel == null)
            {
                return BadRequest("Cart model cannot be null.");
            }
            try
            {
                var updatedCart = await _cartsService.UpdateCartAsync(CartId, cartModel);
                return Ok(updatedCart);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Cart with ID {CartId} not found.");
            }
        }
        [HttpDelete("{CartId}")]
        public async Task<IActionResult> DeleteCart(int CartId)
        {
            try
            {
                var result = await _cartsService.DeleteCartAsync(CartId);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Cart with ID {CartId} not found.");
            }
        }

    }
}
