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

        private int HitBy(int life, int armor, int dmg)
        {
            if (dmg <= 1)
                dmg++;
            return dmg - (armor * 10 / 100);
        }

        private int GetNumberOfHits(int round, decimal attackSpeed)
        {   // R * AS - ( R - 1 ) * AS   R - round, AS - attackspeed
            int tempAttackSpeed = (int)Decimal.Ceiling(attackSpeed);
            return round * tempAttackSpeed - (round - 1) * tempAttackSpeed;
        }

        public string GetBattleReport(CharacterViewModel characterViewModel, Monster monster, ref bool characterWinner)
        {
            int round = 0;
            string raport = "";
            int finalDMG = 0;

            if (characterViewModel.Level < monster.Level)
            {
                characterViewModel.ChanceToHit = characterViewModel.ChanceToHit - 5;
            }

            if (monster.AttackSpeed < characterViewModel.AttackSpeed)
            { // atak characteru 
                raport += String.Format("\n <name>{0}</name> rozpoczął walkę.", characterViewModel.Name);
            }
            else
            { // atak monstera
                raport += String.Format("\nZaatakował Cię {0}", monster.Name);
            }

            Random dice = new Random();
            int intervalOfChanceToHit;

            do
            {
                round++;
                for (int i = 0; i < GetNumberOfHits(round, characterViewModel.AttackSpeed); i++)
                {
                    intervalOfChanceToHit = dice.Next(1, 101);
                    if (characterViewModel.ChanceToHit >= intervalOfChanceToHit)
                    {
                        finalDMG = HitBy(monster.Life, monster.Defense, dice.Next(characterViewModel.MinDamage, characterViewModel.MaxDamage + 1));
                        monster.Life = monster.Life - finalDMG;

                        if (monster.Life <= 0)
                        {
                            break;
                        }

                        raport += String.Format("\n{2} zaatakował {0} dmg, życie przeciwnika wynosi: {1}", finalDMG, monster.Life, characterViewModel.Name);
                    }
                    else
                    {
                        raport += String.Format("\nNie trafiłeś przeciwnika.");
                    }
                }

                for (int i = 0; i < GetNumberOfHits(round, monster.AttackSpeed); i++)
                {
                    intervalOfChanceToHit = dice.Next(1, 101);
                    if (monster.ChanceToHit >= intervalOfChanceToHit)
                    {
                        finalDMG = HitBy(characterViewModel.Life, characterViewModel.Armor, dice.Next(monster.MinDamage, monster.MaxDamage + 1));
                        characterViewModel.Life = characterViewModel.Life - finalDMG;

                        if (characterViewModel.Life <= 0)
                        {
                            break;
                        }

                        raport += String.Format("\n{2} zaatakował {0} dmg, życie Twojej postaci wynosi: {1}", finalDMG, characterViewModel.Life, monster.Name);
                    }
                    else
                    {
                        raport += String.Format("\nPrzeciwnik spudłował.");
                    }
                }
            }
            while (monster.Life > 0 && characterViewModel.Life > 0);

            if (monster.Life == 0 || monster.Life < 0)
            {
                raport += String.Format("\nWygrałeś bitwę.");
                characterWinner = true;
            }

            if (characterViewModel.Life == 0 || characterViewModel.Life < 0)
            {
                raport += String.Format("\nPrzeciwnik wygrał walkę.");
                characterWinner = false;
            }

            return raport;
        }
    }
}
