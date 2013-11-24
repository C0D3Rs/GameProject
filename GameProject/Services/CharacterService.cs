using GameProject.Enums;
using GameProject.Models;
using GameProject.Models.Entities;
using GameProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameProject.Services
{
    public class CharacterService
    {
        public CharacterViewModel GetCharacterViewModel(Character character, List<ItemViewModel> items)
        {
            CharacterViewModel characterViewModel = new CharacterViewModel();

            characterViewModel.Class = character.Class;
            characterViewModel.Name = character.Name;
            
            characterViewModel.Experience = character.Experience;
            characterViewModel.ExperienceToLevel = (characterViewModel.Level + 1) * 100;
            characterViewModel.Level = characterViewModel.Experience / 100 + 1;

            if (characterViewModel.Class == CharacterClass.Warrior)
            {
                characterViewModel.Strength += 3 * characterViewModel.Level;
                characterViewModel.Dexterity += 1 * characterViewModel.Level;
                characterViewModel.Intelligence += 1 * characterViewModel.Level;
            }
            else if (characterViewModel.Class == CharacterClass.Archer)
            {
                characterViewModel.Strength += 1 * characterViewModel.Level;
                characterViewModel.Dexterity += 3 * characterViewModel.Level;
                characterViewModel.Intelligence += 1 * characterViewModel.Level;
            }
            else if (characterViewModel.Class == CharacterClass.Mage)
            {
                characterViewModel.Strength += 1 * characterViewModel.Level;
                characterViewModel.Dexterity += 1 * characterViewModel.Level;
                characterViewModel.Intelligence += 3 * characterViewModel.Level;
            }

            characterViewModel.Vitality += 2 * characterViewModel.Level;

            ItemViewModel weapon = items.FirstOrDefault(i => i.Item.Type == ItemType.Weapon);
            List<ItemViewModel> armorsAndShields = items.FindAll(i => i.Item.Type == ItemType.Shield || i.Item.Type == ItemType.Armor);

            characterViewModel.Strength += items.Sum(gi => gi.GeneratedItem.Strength);
            characterViewModel.Dexterity += items.Sum(gi => gi.GeneratedItem.Dexterity);
            characterViewModel.Intelligence += items.Sum(gi => gi.GeneratedItem.Intelligence);
            characterViewModel.Vitality += items.Sum(gi => gi.GeneratedItem.Vitality);

            characterViewModel.MinDamage = (weapon != null ? weapon.GeneratedItem.PrimaryMinValue : 1);
            characterViewModel.MaxDamage = (weapon != null ? weapon.GeneratedItem.PrimaryMaxValue : 2);
            characterViewModel.Armor = armorsAndShields.Sum(aas => aas.GeneratedItem.PrimaryMinValue);

            characterViewModel.Life = characterViewModel.Vitality * 10 * characterViewModel.Level;
            characterViewModel.ChanceToHit = 30 * characterViewModel.Dexterity / characterViewModel.Level + 10;
            characterViewModel.AttackSpeed = characterViewModel.Dexterity / 100 + 1;

            if (characterViewModel.ChanceToHit > 75)
            {
                characterViewModel.ChanceToHit = 75;
            }
            
            return characterViewModel;
        }
    }
}
