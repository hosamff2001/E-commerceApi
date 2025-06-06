using EcommerceApi.Models.Products;

namespace EcommerceApi.Services.Products
{
    public interface IProductService
    {
        Task<List<ProductModel>> GetAllProducts();
        Task<ProductModel> GetProductById(int id);
        Task<ProductModel> CreatProduct(ProductModelDto product);
        Task<ProductModel> UpdateProduct(int id,ProductModelDto product);
        Task<ProductModel> DeleteProduct(int id);
    }
}
