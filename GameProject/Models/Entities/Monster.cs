using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameProject.Models.Entities
{
    public class Monster
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Level { get; set; }

        [Required]
        public int Life { get; set; }

        [Required]
        public int MinDamage { get; set; }

        [Required]
        public int MaxDamage { get; set; }

        [Required]
        public int Defense { get; set; }

        [Timestamp]
        public byte[] CurrentVersion { get; set; }
    }
}
