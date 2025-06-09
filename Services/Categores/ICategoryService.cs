using EcommerceApi.Models.Categores;

namespace EcommerceApi.Services.Categores
{
    public interface ICategoryService
    {
        Task<(List<CategoryModel>,int)> GetAllCategories(int pageNumber, int pageSize);
        Task<CategoryModel> GetCategoryById(int id);
        Task<CategoryModel> CreateCategory(CategoryModelDto category);
        Task<CategoryModel> UpdateCategory(int id, CategoryModelDto category);
        Task<CategoryModel> DeleteCategory(int id);
    }
}
