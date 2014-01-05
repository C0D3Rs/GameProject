using GameProject.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameProject.Areas.Admin.ViewModels
{
    public class CreateEventViewModel
    {
        public Location Location { get; set; }
        public Event Event { get; set; }
        public List<DetailsMonsterViewModel> Monsters { get; set; }
    }
}
