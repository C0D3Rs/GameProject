using GameProject.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameProject.Models.Entities
{
    public class EventLog
    {
        public int Id { get; set; }

        [Required]
        public int CharacterId { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required]
        public DateTime Created_at { get; set; }

        [Timestamp]
        public byte[] CurrentVersion { get; set; }
    }
}
