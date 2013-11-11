using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameProject.Models.Entities;

namespace GameProject.ViewModels
{
    public class EquipmentViewModel
    {
        public List<GeneratedItem> EquippedItems;
        public List<GeneratedItem> BackpackItems;
        public List<GeneratedItem> ChestItems;
    }
}
