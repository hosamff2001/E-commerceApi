using EcommerceApi.Models.Pagination;
using EcommerceApi.Models.Products;
using EcommerceApi.Services.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpGet]
      //  [AllowAnonymous]
        public async Task<IActionResult> GetAllProducts(
          [FromQuery] int pageNumber = 1,
          [FromQuery] int pageSize = 10)
        {
            // Validate page size (you might want to set a maximum)
            pageSize = Math.Min(pageSize, 100);

            var (products, totalRecords) = await _productService.GetAllProducts(pageNumber, pageSize);

            var response = new PagedResponse<ProductModel>(
                products,
                pageNumber,
                pageSize,
                totalRecords);

            return Ok(response);
        }

        [HttpGet("{id}")]
       // [AllowAnonymous]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
       // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductModelDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdProduct = await _productService.CreatProduct(productDto);

            return CreatedAtAction(
                nameof(GetProductById),
                new { id = createdProduct.product_id },
                createdProduct);
        }

        [HttpPut("{id}")]
      //  [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(
            [FromRoute] int id,
            [FromForm] ProductModelDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingProduct = await _productService.GetProductById(id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            var updatedProduct = await _productService.UpdateProduct(id, productDto);

            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
      //  [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var existingProduct = await _productService.GetProductById(id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            await _productService.DeleteProduct(id);

            return NoContent();
        }
    }
}