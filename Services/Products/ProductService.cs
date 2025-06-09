using EcommerceApi.Models.DbContext;
using EcommerceApi.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDBContext context;
        private readonly List<string> allowedextension = new List<string> { ".jpg", ".png" };
        private readonly long maxlength = 1048576;

        public ProductService(ApplicationDBContext applicationDBContext)
        {
            context = applicationDBContext;
        }
        public async Task<ProductModel> CreatProduct(ProductModelDto product)
        {
            // Validate category exists
            var categoryExists = await context.CategoryModel.AnyAsync(g => g.category_id == product.category_id);
            if (!categoryExists)
                throw new ArgumentException("Invalid Category Id");

            var newProduct = new ProductModel
            {
                product_name = product.product_name,
                product_description = product.product_description,
                price = product.price,
                category_id = product.category_id,
                brand = product.brand,
                color = product.color,
            };

            // Process image1 if provided
            if (product.image1 != null)
            {
                if (!allowedextension.Contains(Path.GetExtension(product.image1.FileName).ToLower()))
                    throw new ArgumentException("Invalid Extension for Image1");

                if (product.image1.Length > maxlength)
                    throw new ArgumentException("Invalid Length for Image1");

                using var datastream1 = new MemoryStream();
                await product.image1.CopyToAsync(datastream1);
                newProduct.image1 = datastream1.ToArray();
            }

            // Process image2 if provided
            if (product.image2 != null)
            {
                if (!allowedextension.Contains(Path.GetExtension(product.image2.FileName).ToLower()))
                    throw new ArgumentException("Invalid Extension for Image2");

                if (product.image2.Length > maxlength)
                    throw new ArgumentException("Invalid Length for Image2");

                using var datastream2 = new MemoryStream();
                await product.image2.CopyToAsync(datastream2);
                newProduct.image2 = datastream2.ToArray();
            }

            // Process image3 if provided
            if (product.image3 != null)
            {
                if (!allowedextension.Contains(Path.GetExtension(product.image3.FileName).ToLower()))
                    throw new ArgumentException("Invalid Extension for Image3");

                if (product.image3.Length > maxlength)
                    throw new ArgumentException("Invalid Length for Image3");

                using var datastream3 = new MemoryStream();
                await product.image3.CopyToAsync(datastream3);
                newProduct.image3 = datastream3.ToArray();
            }

            await context.AddAsync(newProduct);
            await context.SaveChangesAsync();
            return newProduct;
        }

        public async Task<ProductModel> DeleteProduct(int id)
        {
            var product = await context.ProductModel.FirstOrDefaultAsync(p => p.product_id == id);
            if (product == null)
                return null;
            context.ProductModel.Remove(product);
            await context.SaveChangesAsync();
            return product;
        }

        public async Task<List<ProductModel>> GetAllProducts()
        {
            List<ProductModel> productModels = await context.ProductModel.ToListAsync();
            return productModels;
        }

        public async Task<ProductModel> GetProductById(int id)
        {
            var product = await context.ProductModel.FirstOrDefaultAsync(p => p.product_id == id);
            return product;
        }

        public async Task<ProductModel> UpdateProduct(int id, ProductModelDto product)
        {
            var existingProduct = await context.ProductModel.FirstOrDefaultAsync(p => p.product_id == id);
            if (existingProduct == null)
                return null;
            existingProduct.product_name = product.product_name;
            existingProduct.product_description = product.product_description;
            existingProduct.price = product.price;
            existingProduct.category_id = product.category_id;
            existingProduct.brand = product.brand;
            existingProduct.color = product.color;

            // Process image1 if provided
            if (product.image1 != null)
            {
                if (!allowedextension.Contains(Path.GetExtension(product.image1.FileName).ToLower()))
                    throw new ArgumentException("Invalid Extension for Image1");

                if (product.image1.Length > maxlength)
                    throw new ArgumentException("Invalid Length for Image1");

                using var datastream1 = new MemoryStream();
                await product.image1.CopyToAsync(datastream1);
                existingProduct.image1 = datastream1.ToArray();
            }

            // Process image2 if provided
            if (product.image2 != null)
            {
                if (!allowedextension.Contains(Path.GetExtension(product.image2.FileName).ToLower()))
                    throw new ArgumentException("Invalid Extension for Image2");

                if (product.image2.Length > maxlength)
                    throw new ArgumentException("Invalid Length for Image2");

                using var datastream2 = new MemoryStream();
                await product.image2.CopyToAsync(datastream2);
                existingProduct.image2 = datastream2.ToArray();
            }

            // Process image3 if provided
            if (product.image3 != null)
            {
                if (!allowedextension.Contains(Path.GetExtension(product.image3.FileName).ToLower()))
                    throw new ArgumentException("Invalid Extension for Image3");

                if (product.image3.Length > maxlength)
                    throw new ArgumentException("Invalid Length for Image3");

                using var datastream3 = new MemoryStream();
                await product.image3.CopyToAsync(datastream3);
                existingProduct.image3 = datastream3.ToArray();
            }
            context.ProductModel.Update(existingProduct);
            await context.SaveChangesAsync();
            return existingProduct;
        }
    }
}
