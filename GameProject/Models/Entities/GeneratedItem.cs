using GameProject.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameProject.Models.Entities
{
    public class GeneratedItem
    {
        public int Id { get; set; }

        [Required]
        public int CharacterId { get; set; }

        [Required]
        public int ItemId { get; set; }

        public int PrefixId { get; set; }

        public int SuffixId { get; set; }

        [Required]
        public ItemStatus Status { get; set; }

        public int PrimaryMinValue { get; set; }

        public int PrimaryMaxValue { get; set; }

        public int Strength { get; set; }

        public int Dexterity { get; set; }

        public int Intelligence { get; set; }

        public int Vitality { get; set; }

        [Required]
        public int Durability { get; set; }

        [Timestamp]
        public byte[] CurrentVersion { get; set; }
    }
}
