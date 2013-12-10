using GameProject.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameProject.Models.Entities
{
    public class Character
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public CharacterClass Class { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Nazwa musi posiadać minimum 5 znaków i nie może przekraczać 20 znaków.")]
        public string Name { get; set; }

        public int Experience { get; set; }

        public int Gold { get; set; }

        public int AvailableMoves { get; set; }

        public DateTime RenewalTime { get; set; }
    }
}
