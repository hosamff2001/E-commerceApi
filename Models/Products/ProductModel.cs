using EcommerceApi.Models.Categores;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApi.Models.Products
{
    public class ProductModel
    {
        [Key]
        [Required]

        public int product_id { get; set; }
        [Required]
        [MaxLength(50)]
        public string product_name { get; set; }
        [Required]
        [MaxLength(250)]
        public string product_description { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public float price { get; set; }
        [Required]
        [MaxLength(50)]
        public string brand { get; set; }
        [Required]
        [MaxLength(20)]
        public string color { get; set; }
        public byte[]? image1 { get; set; }
        public byte[]? image2 { get; set; }
        public byte[]? image3 { get; set; }
        [ForeignKey("category")]
        public int category_id { get; set; }

        public CategoryModel category { get; set; }

    }
}
