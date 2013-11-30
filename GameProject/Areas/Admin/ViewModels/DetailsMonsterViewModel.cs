using GameProject.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameProject.Areas.Admin.ViewModels
{
    public class DetailsMonsterViewModel
    {
        public Monster Monster { get; set; }
        public Image Image { get; set; }
    }
}
