using EcommerceApi.Models.Categores;
using EcommerceApi.Models.Pagination;
using EcommerceApi.Services.Categores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        //[AllowAnonymous]
        public async Task<IActionResult> GetAllCategories(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
            )
        {
            pageSize = Math.Min(pageSize, 100); 
            var (categories, totalRecords) = await _categoryService.GetAllCategories(pageNumber, pageSize);
            var response = new PagedResponse<CategoryModel>(
                categories,
                pageNumber,
                pageSize,
                totalRecords);
            return Ok(response);

        }

        [HttpGet("{id}")]
        //[AllowAnonymous]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryById(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryModelDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdCategory = await _categoryService.CreateCategory(categoryDto);

            return CreatedAtAction(
                nameof(GetCategoryById),
                new { id = createdCategory.category_id },
                createdCategory);
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(
            [FromRoute] int id,
            [FromBody] CategoryModelDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCategory = await _categoryService.GetCategoryById(id);

            if (existingCategory == null)
            {
                return NotFound();
            }

            var updatedCategory = await _categoryService.UpdateCategory(id, categoryDto);

            return Ok(updatedCategory);
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var existingCategory = await _categoryService.GetCategoryById(id);

            if (existingCategory == null)
            {
                return NotFound();
            }

            await _categoryService.DeleteCategory(id);

            return NoContent();
        }
    }
}