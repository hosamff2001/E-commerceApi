﻿using System.ComponentModel.DataAnnotations;

namespace EcommerceApi.Models.User
{
    public class RoleModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}
