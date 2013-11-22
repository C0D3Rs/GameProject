using GameProject.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameProject.ViewModels
{
    public class ItemViewModel
    {
        public GeneratedItem GeneratedItem { get; set; }
        public Item Item { get; set; }
        public Image Image { get; set; }
    }
}
