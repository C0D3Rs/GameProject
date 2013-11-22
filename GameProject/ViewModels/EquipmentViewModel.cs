using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameProject.Models.Entities;

namespace GameProject.ViewModels
{
    public class EquipmentViewModel
    {
        public List<ItemViewModel> EquippedItems;
        public List<ItemViewModel> BackpackItems;
        public List<ItemViewModel> ChestItems;
    }
}
