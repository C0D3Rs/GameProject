using GameProject.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameProject.Models.Entities
{
    public class Event
    {
        public int Id { get; set; }

        public int MonsterId { get; set; }

        [Required]
        public int LocationId { get; set; }

        [Required]
        public EventType Type { get; set; }

        [Required]
        public string Name { get; set; }

        // opisy
        [Required]
        [DataType(DataType.MultilineText)]
        public string MainDescription  { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string SuccessDescription { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string LostDescription { get; set; }

        // nagroda
        [Required]
        public int Reward { get; set; }

        // wymagania dla postaci
        [Required]
        public int RequireStrength { get; set; }

        [Required]
        public int RequireDexterity { get; set; }

        [Required]
        public int RequireVitality { get; set; }

        [Required]
        public int RequireInteligence { get; set; }

        [Timestamp]
        public byte[] CurrentVersion { get; set; }
    }
}
