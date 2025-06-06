using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EcommerceApi.Models.User
{
    public class ApplicationUser : IdentityUser
    {
        [Required, StringLength(100)]
        public required string firstName { get; set; }
        [Required, StringLength(100)]
        public required string lastName { get; set; }

        public List<RefrachedToken> refrachedTokens { get; set; } = new List<RefrachedToken>();
    }
}
