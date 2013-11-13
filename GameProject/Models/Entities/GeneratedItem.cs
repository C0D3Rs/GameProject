using GameProject.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameProject.Models.Entities
{
    // przedmioty należące do graczy

    public class GeneratedItem
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public ItemStatus Status { get; set; }

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

        // główne właściwości przedmiotu
        public int PrimaryMinValue { get; set; }

        public int PrimaryMaxValue { get; set; }

        // właściwości affix'ów

        public int Strength { get; set; }

        public int Dexterity { get; set; }

        public int Intelligence { get; set; }

        public int Vitality { get; set; }

        // wymagania do noszenia

        [Required]
        public int RequireStrength { get; set; }

        [Timestamp]
        public byte[] CurrentVersion { get; set; }
    }
}
