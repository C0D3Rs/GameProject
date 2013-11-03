using GameProject.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameProject.Models.Entities
{
    public class Armor
    {
        public int Id { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        public int MinArmorClass { get; set; }

        [Required]
        public int MaxArmorClass { get; set; }

        [Required]
        public int RequireStrength { get; set; }
    }
}
