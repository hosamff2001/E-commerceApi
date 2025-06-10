using AutoMapper;
using EcommerceApi.Models.DbContext;
using EcommerceApi.Models.Products;
using EcommerceApi.Services.UploadFilesSrvice;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDBContext context;
        private readonly IUploadFilesService uploadFilesService;
        private readonly IMapper mapper;

        public ProductService(ApplicationDBContext applicationDBContext,IUploadFilesService filesService,IMapper mapper)
        {
            context = applicationDBContext;
            uploadFilesService = filesService;
            this.mapper = mapper;
        }
        public async Task<ProductModel> CreatProduct(ProductModelDto product)
        {
            // Validate category exists
            var categoryExists = await context.CategoryModel.AnyAsync(g => g.category_id == product.category_id);
            if (!categoryExists)
                throw new ArgumentException("Invalid Category Id");

            var newProduct = mapper.Map<ProductModel>(product);

            // Process image1 if provided
            if (product.image1 != null)
            {
                newProduct.image1 = await uploadFilesService.UploadFileAsync(product.image1);
            }

            // Process image2 if provided
            if (product.image2 != null)
            {
                newProduct.image2 = await uploadFilesService.UploadFileAsync(product.image2);
            }

            // Process image3 if provided
            if (product.image3 != null)
            { 
                newProduct.image3 = await uploadFilesService.UploadFileAsync(product.image3);
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

        public async Task<(List<ProductModel>, int)> GetAllProducts(int pageNumber, int pageSize)
        {
            var query = context.ProductModel.AsQueryable();

            // Get total count before pagination
            var totalRecords = await query.CountAsync();

            // Apply pagination
            var products = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (products, totalRecords);
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
                existingProduct.image1 = await uploadFilesService.UploadFileAsync(product.image1);
            }

            // Process image2 if provided
            if (product.image2 != null)
            {
                existingProduct.image2 = await uploadFilesService.UploadFileAsync(product.image2);
            }

            // Process image3 if provided
            if (product.image3 != null)
            {
                existingProduct.image3 = await uploadFilesService.UploadFileAsync(product.image3);
            }
            context.ProductModel.Update(existingProduct);
            await context.SaveChangesAsync();
            return existingProduct;
        }
    }
}
