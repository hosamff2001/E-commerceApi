namespace EcommerceApi.Models.Products
{
    public class ProductModelDto
    {
        public string product_name { get; set; }
        public string product_description { get; set; }
        public float price { get; set; }
        public int category_id { get; set; }
        public string brand { get; set; }
        public string color { get; set; }
        public IFormFile? image1 { get; set; }
        public IFormFile? image2 { get; set; }
        public IFormFile? image3 { get; set; }
    }
}
