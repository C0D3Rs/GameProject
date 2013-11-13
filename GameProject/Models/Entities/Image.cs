using GameProject.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameProject.Models.Entities
{
    public class Image
    {
        public int ID { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public byte[] Data { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public ImageCategory Category { get; set; }

        [Timestamp]
        public byte[] CurrentVersion { get; set; }
    }
}
