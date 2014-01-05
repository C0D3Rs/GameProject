using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameProject.Models.Entities
{
    public class Message
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string FromUser { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime Date { get; set; }
        [DataType(DataType.MultilineText)]
        [Required]
        public string ContentOfMessage { get; set; }
        [Required]
        public int ToUserId { get; set; }
    }
}