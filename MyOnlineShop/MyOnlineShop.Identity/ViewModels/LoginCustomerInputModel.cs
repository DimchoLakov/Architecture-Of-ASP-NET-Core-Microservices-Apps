﻿using System.ComponentModel.DataAnnotations;

namespace MyOnlineShop.Identity.ViewModels
{
    public class LoginCustomerInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
