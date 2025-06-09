using System.ComponentModel.DataAnnotations;

namespace EcommerceApi.Models.Categores
{
    public class CategoryModel
    {
        [Key]
        [Required]

        public int category_id { get; set; }
        [Required]
        [MaxLength(50)]
        public string category_name { get; set; }
        [Required]
        [MaxLength(250)]
        public string category_description { get; set; }

    }
}
