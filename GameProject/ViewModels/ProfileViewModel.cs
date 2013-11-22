using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameProject.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace GameProject.ViewModels
{
    public class ProfileViewModel
    {
        public string Name { get; set; }
        public int Experience { get; set; }
        public int Gold { get; set; }

        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Intelligence { get; set; }
        public int Vitality { get; set; }
    }
}
