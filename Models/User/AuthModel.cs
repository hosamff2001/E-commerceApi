using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EcommerceApi.Models.User
{
    public class AuthModel
    {
        [Required]
        public string Message { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public bool IsAuthenticated { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public List<string> Roles { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
