using GameProject.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameProject.Models.Entities
{
    public class Item
    {
        public int Id { get; set; }

        [Required]
        public ItemType Type { get; set; }

        [Required]
        public SubType SubType { get; set; }

        [Required]
        public string Name { get; set; }

        // właściwości

        public int MinDamage { get; set; }

        public int MaxDamage { get; set; }

        public int MinArmor { get; set; }

        public int MaxArmor { get; set; }

        // wymagania dla postaci noszącej

        [Required]
        public int RequireStrength { get; set; }

        [Required]
        public int QualityLevel { get; set; }

        [Required]
        public int Price { get; set; }

        [Timestamp]
        public byte[] CurrentVersion { get; set; }
    }
}
