using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameProject.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace GameProject.ViewModels
{
    public class LoginUserViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Wpisz poprawny adres e-mail.")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
    }
}
