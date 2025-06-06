﻿using System.ComponentModel.DataAnnotations;

namespace EcommerceApi.Models.User
{
    public class SignInModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
