using EcommerceApi.Models.Categores;
using EcommerceApi.Models.DbContext;

namespace EcommerceApi.Services.Categores
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDBContext context;
        public CategoryService(ApplicationDBContext applicationDBContext)
        {
            context = applicationDBContext;
        }
        public Task<CategoryModel> CreateCategory(CategoryModelDto category)
        {
            var newCategory = new CategoryModel
            {
                category_name = category.Category_name,
                category_description = category.Category_description
            };
            context.Add(newCategory);
            context.SaveChanges();
            return Task.FromResult(newCategory);
        }

        public Task<CategoryModel> DeleteCategory(int id)
        {
            var category = context.CategoryModel.FirstOrDefault(c => c.category_id == id);
            if (category == null)
                return null;
            context.CategoryModel.Remove(category);
            context.SaveChanges();
            return Task.FromResult(category);
        }

        public Task<List<CategoryModel>> GetAllCategories()
        {
            var categories = context.CategoryModel.ToList();
            
            return Task.FromResult(categories);
        }

        public Task<CategoryModel> GetCategoryById(int id)
        {
            var category = context.CategoryModel.FirstOrDefault(c => c.category_id == id);
            return Task.FromResult(category);
        }

        public Task<CategoryModel> UpdateCategory(int id, CategoryModelDto category)
        {
            
            var existingCategory = context.CategoryModel.FirstOrDefault(c => c.category_id == id);
            if (existingCategory == null)
                return null;
            existingCategory.category_name = category.Category_name;
            existingCategory.category_description = category.Category_description;
            context.SaveChanges();
            return Task.FromResult(existingCategory);
        }
    }
}
