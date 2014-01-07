using GameProject.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameProject.Areas.Admin.ViewModels
{
    public class DetailsEventViewModel
    {
        public Location Location { get; set; }
        public Event Event { get; set; }
        public Monster Monster { get; set; }
        public Image LocationImage { get; set; }
        public Image MonsterImage { get; set; }
    }
}
