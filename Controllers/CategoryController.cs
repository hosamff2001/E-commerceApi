using EcommerceApi.Models.Categores;
using EcommerceApi.Services.Categores;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
       private readonly ICategoryService service;
        public CategoryController(ICategoryService service)
        {
            this.service = service;
        }
        [HttpGet("Get_All_Categories")]
        public async Task<IActionResult> GetAll()
        {
            List<CategoryModel> categories = await service.GetAllCategories();
            return Ok(categories);
        }
        [HttpGet("Get_Category_By_Id")]
        public async Task<IActionResult> GetById(int id)
        {
            CategoryModel category = await service.GetCategoryById(id);
            return Ok(category);
        }
        [HttpPut("Update_Category")]
        public async Task<IActionResult> Update(int id, CategoryModelDto category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            CategoryModel updatedCategory = await service.UpdateCategory(id, category);
            return Ok(updatedCategory);
        }
        [HttpPost("Create_Category")]
        public async Task<IActionResult> Create( CategoryModelDto category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            CategoryModel newCategory = await service.CreateCategory(category);
            return CreatedAtAction(nameof(GetById), new { id = newCategory.category_id }, newCategory);
        }
        [HttpDelete("Delete_Category")]
        public async Task<IActionResult> Delete(int id)
        {
            CategoryModel deletedCategory = await service.DeleteCategory(id);
            return Ok(deletedCategory);
        }
    }
}
