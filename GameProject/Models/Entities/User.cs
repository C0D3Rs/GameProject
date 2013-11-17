using GameProject.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameProject.Models.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public UserRole Role { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        [Timestamp]
        public byte[] CurrentVersion { get; set; }
    }
}
