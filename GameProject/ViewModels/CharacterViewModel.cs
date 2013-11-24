using GameProject.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameProject.ViewModels
{
    public class CharacterViewModel
    {
        public CharacterClass Class { get; set; }

        public string Name { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public int ExperienceToLevel { get; set; }

        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public int Armor { get; set; }

        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Intelligence { get; set; }
        public int Vitality { get; set; }

        public int Life { get; set; }
        public int ChanceToHit { get; set; }
        public decimal AttackSpeed { get; set; }
    }
}
