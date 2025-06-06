using EcommerceApi.Models.Categores;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApi.Models.Products
{
    public class ProductModel
    {
        [Key]
        public int product_id { get; set; }
        public string product_name { get; set; }
        public string product_description { get; set; }
        public float price { get; set; }
        public string brand { get; set; }
        public string color { get; set; }
        public byte[]? image1 { get; set; }
        public byte[]? image2 { get; set; }
        public byte[]? image3 { get; set; }
        [ForeignKey("category")]
        public int category_id { get; set; }

        public CategoryModel category { get; set; }

    }
}
