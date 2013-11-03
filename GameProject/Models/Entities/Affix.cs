using GameProject.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameProject.Models.Entities
{
    public class Affix
    {
        public int Id { get; set; }

        // rodzaj affix'u (np. przedrostek, przyrostek)

        [Required]
        public AffixType Type { get; set; }

        [Required]
        public string Name { get; set; }

        // dla jakich przedmiotów występuje

        [Required]
        public bool ForWeapon { get; set; }

        [Required]
        public bool ForShield { get; set; }

        [Required]
        public bool ForArmor { get; set; }

        [Required]
        public bool ForJewelry { get; set; }

        // możliwe właściwości affix'ów

        [Required]
        public int MinStrength { get; set; }

        [Required]
        public int MaxStrength { get; set; }

        // poziom jakości affix'u

        [Required]
        public int QualityLevel { get; set; }

        [Required]
        public int Price { get; set; }

        [Timestamp]
        public byte[] CurrentVersion { get; set; }
    }
}
