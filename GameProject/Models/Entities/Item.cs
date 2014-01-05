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

        public int ImageId { get; set; }

        [Required]
        public ItemType Type { get; set; }

        [Required]
        public SubType SubType { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int QualityLevel { get; set; }

        [Required]
        public int Durability { get; set; }

        [Required]
        public int Price { get; set; }

        // główny atrybut dla przedmiotów (obrażenia / pancerz)

        public int PrimaryMinValue { get; set; }

        public int PrimaryMaxValue { get; set; }

        // wymagania dla postaci noszącej

        [Required]
        public int RequireStrength { get; set; }

        public decimal AttackSpeed { get; set; }
        public int ChanceToHit { get; set; }

        [Timestamp]
        public byte[] CurrentVersion { get; set; }
    }
}
