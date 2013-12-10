using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameProject.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace GameProject.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Wpisz poprawny adres e-mail.")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "Hasło, które wprowadziłeś różni się od hasła potwierdzającego.")]
        public string ConfirmPassword { get; set; }
    }
}
