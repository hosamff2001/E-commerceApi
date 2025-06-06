using System.ComponentModel.DataAnnotations;

namespace EcommerceApi.Models.Categores
{
    public class CategoryModel
    {
        [Key]
        public int category_id { get; set; }
        public string category_name { get; set; }
        public string category_description { get; set; }

    }
}
