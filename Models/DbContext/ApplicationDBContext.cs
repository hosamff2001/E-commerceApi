using EcommerceApi.Models.Categores;
using EcommerceApi.Models.Products;
using EcommerceApi.Models.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace EcommerceApi.Models.DbContext
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
           : base(options)
        {
        }
        public DbSet<ProductModel> ProductModel { get; set; }
        public DbSet<CategoryModel> CategoryModel { get; set; }

    }
}
