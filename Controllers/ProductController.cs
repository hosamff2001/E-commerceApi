using EcommerceApi.Models.Products;
using EcommerceApi.Services.Products;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService service;
        public ProductController(IProductService service)
        {
            this.service = service;
        }
        [HttpGet("Get_All_Products")]
        public async Task<IActionResult> GetAll()
        {
            List<ProductModel> products = await service.GetAllProducts();
            return Ok(products);
        }
        [HttpGet("Get_Product_By_Id")]
        public async Task<IActionResult> GetById(int id)
        {
            ProductModel product = await service.GetProductById(id);
            return Ok(product);
        }
        [HttpPut("Update_Product")]
        public async Task<IActionResult> Update(int id, [FromForm] ProductModelDto product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ProductModel updatedProduct = await service.UpdateProduct(id, product);
            return Ok(updatedProduct);
        }
        [HttpPost("Create_Product")]
        public async Task<IActionResult> Create([FromForm] ProductModelDto product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ProductModel newProduct = await service.CreatProduct(product);
            return CreatedAtAction(nameof(GetById), new { id = newProduct.product_id }, newProduct);
        }
        [HttpDelete("Delete_Product")]
        public async Task<IActionResult> Delete(int id)
        {
            ProductModel deletedProduct = await service.DeleteProduct(id);
            return Ok(deletedProduct);

        }
    }
}
