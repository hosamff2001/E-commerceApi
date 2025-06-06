using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Models.User
{
    [Owned]
    public class RefrachedToken
    {
        public string Token { get; set; }
        public DateTime ExpiredOn { get; set; }
        public bool IsExpied => DateTime.Now >= ExpiredOn;
        public DateTime CreatedOn { get; set; }
        public DateTime? RevokedOn { get; set; }
        public bool IsActived => RevokedOn is null && !IsExpied;
    }
}
