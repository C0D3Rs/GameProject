using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameProject.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace GameProject.ViewModels
{
    public class LoginUserViewModel
    {
        [Required]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
    }
}
