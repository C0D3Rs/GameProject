using GameProject.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameProject.Models.Entities
{
    public class Weapon
    {
        public int Id { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        public WeaponType Type { get; set; }

        [Required]
        public int MinDamage { get; set; }

        [Required]
        public int MaxDamage { get; set; }

        [Required]
        public int RequireStrength { get; set; }
    }
}
