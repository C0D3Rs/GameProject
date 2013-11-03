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
        public string Name { get; set; }

        [Required]
        public int QualityLevel { get; set; }

        [Required]
        public int Price { get; set; }
    }
}
