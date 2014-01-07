using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using GameProject.Enums;
using System.Web.Mvc;

namespace GameProject.Models.Entities
{
    public class Message
    {
        public int Id { get; set; }
        [Required]
        [AllowHtml]
        public string Title { get; set; }
        public string FromUser { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy HH:mm:ss}")]
        public DateTime Date { get; set; }
        [DataType(DataType.MultilineText)]
        [Required]
        [AllowHtml]
        public string ContentOfMessage { get; set; }
        [Required]
        public int ToUserId { get; set; }
        [Required]
        public MessageType Type { get; set; }
    }
}