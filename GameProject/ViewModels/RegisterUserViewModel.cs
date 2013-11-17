using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameProject.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace GameProject.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Hasło, które wprowadziłeś różni się od hasła potwierdzającego.")]
        public string ConfirmPassword { get; set; }
    }
}
