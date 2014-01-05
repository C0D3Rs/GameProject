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
            characterViewModel.ChanceToHit = weapon.Item.ChanceToHit * characterViewModel.Dexterity / characterViewModel.Level;
            characterViewModel.AttackSpeed = characterViewModel.Dexterity / 100 + weapon.Item.AttackSpeed;

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
            return (int)(round * attackSpeed - Decimal.Floor((round - 1) * attackSpeed));
        }

        public string GetBattleReport(CharacterViewModel characterViewModel, Monster monster, ref bool characterWinner)
        {
            int round = 0;
            string raport = "";
            int finalDMG = 0;
            bool firstHitByMonster = false;
            int fullLifeOfCharacter = characterViewModel.Life;
            int fullLifeOfMonster = monster.Life;
            int characterChanceToHitMinInterval = 10;
            int characterChanceToHitMaxInterval = 90;

            if (characterViewModel.Level < monster.Level)
            {
                characterViewModel.ChanceToHit = characterViewModel.ChanceToHit - 5;
            }

            // Przedział chance to hit 10 < chance to hit < 90
            if (characterChanceToHitMinInterval > characterViewModel.ChanceToHit)
            {
                characterViewModel.ChanceToHit = characterViewModel.ChanceToHit + 9;
            }

            if (characterViewModel.ChanceToHit > characterChanceToHitMaxInterval)
            {
                characterViewModel.ChanceToHit = characterViewModel.ChanceToHit - 9;
            }

            if (characterChanceToHitMinInterval > monster.ChanceToHit)
            {
                monster.ChanceToHit = monster.ChanceToHit + 9;
            }

            if (monster.ChanceToHit > characterChanceToHitMaxInterval)
            {
                monster.ChanceToHit = monster.ChanceToHit - 9;
            }

            if (monster.AttackSpeed > characterViewModel.AttackSpeed)
            { // atak characteru 
                firstHitByMonster = true;
            }

            Random dice = new Random();
            int intervalOfChanceToHit;

            do
            {
                round++;
                raport += String.Format("<round><roundvalue>{0}</roundvalue></round>", round);

                if (firstHitByMonster == true)
                {
                    goto monster;
                }
            character:
                for (int i = 0; i < GetNumberOfHits(round, characterViewModel.AttackSpeed); i++)
                {
                    intervalOfChanceToHit = dice.Next(1, 101);
                    if (characterViewModel.ChanceToHit >= intervalOfChanceToHit)
                    {
                        finalDMG = HitBy(monster.Life, monster.Defense, dice.Next(characterViewModel.MinDamage, characterViewModel.MaxDamage + 1));
                        monster.Life = monster.Life - finalDMG;

                        if (monster.Life > 0)
                        {
                            raport += String.Format("<characterattack><characternamewhenattack>{0}</characternamewhenattack><characterattackmonstername>{1}</characterattackmonstername><characterattackdmg>{2}</characterattackdmg></characterattack>", characterViewModel.Name, monster.Name, finalDMG);
                        }
                        else
                        {
                            raport += String.Format("<characterfinalattack><characternamewhenfinalattack>{0}</characternamewhenfinalattack><characterattackfinaldmg>{1}</characterattackfinaldmg></characterfinalattack>", characterViewModel.Name, finalDMG);
                            goto CharacterWin;
                        }

                    }
                    else
                    {
                        if (monster.Life <= 0 || characterViewModel.Life <= 0)
                            break;

                        raport += String.Format("<charactermissattack><characternamewhenmonsterdododge>{0}</characternamewhenmonsterdododge><charactermonsternamewhenmonsterdododge>{1}</charactermonsternamewhenmonsterdododge></charactermissattack>", characterViewModel.Name, monster.Name);
                    }
                }
                if (firstHitByMonster == true)
                {   // Podsumowanie
                    raport += String.Format("<summation><summationcharactername>{0}</summationcharactername><summationcharacterlifefalldown>{1}</summationcharacterlifefalldown>/<summationcharacterlifeconstantly>{2}</summationcharacterlifeconstantly>", characterViewModel.Name, characterViewModel.Life, fullLifeOfCharacter);
                    raport += String.Format("<summationmonstername>{0}</summationmonstername><summationmonsterlifefalldown>{1}</summationmonsterlifefalldown>/<summationmonsterlifeconstantly>{2}</summationmonsterlifeconstantly></summation>", monster.Name, monster.Life, fullLifeOfMonster);

                    round++;
                    raport += String.Format("<round><roundvalue>{0}</roundvalue></round>", round);
                }


            monster:
                for (int i = 0; i < GetNumberOfHits(round, monster.AttackSpeed); i++)
                {
                    intervalOfChanceToHit = dice.Next(1, 101);
                    if (monster.ChanceToHit >= intervalOfChanceToHit)
                    {
                        finalDMG = HitBy(characterViewModel.Life, characterViewModel.Armor, dice.Next(monster.MinDamage, monster.MaxDamage + 1));
                        characterViewModel.Life = characterViewModel.Life - finalDMG;

                        if (characterViewModel.Life > 0)
                        {
                            raport += String.Format("<monsterattack><monsterattackmonstername>{0}</monsterattackmonstername><monsterattackcharactername>{1}</monsterattackcharactername><monsterattackdmg>{2}</monsterattackdmg></monsterattack>", monster.Name, characterViewModel.Name, finalDMG);
                        }
                        else
                        {
                            raport += String.Format("<monsterfinalattack><monsterfinalattackmonstername>{0}</monsterfinalattackmonstername><monsterfinalattackdmg>{1}</monsterfinalattackdmg></monsterfinalattack>", monster.Name, finalDMG);
                            goto MonsterWin;
                        }
                    }
                    else
                    {
                        if (characterViewModel.Life <= 0 || monster.Life <= 0)
                            break;

                        raport += String.Format("<monstermissattack><monstermissattackmonstername>{0}</monstermissattackmonstername><monstermissattackcharactername>{1}</monstermissattackcharactername></monstermissattack>", monster.Name, characterViewModel.Name);
                    }
                }
                if (firstHitByMonster == true)
                {
                    goto character;
                }
                else
                {   // Podsumowanie
                    raport += String.Format("<summation><summationcharactername>{0}</summationcharactername><summationcharacterlifefalldown>{1}</summationcharacterlifefalldown>/<summationcharacterlifeconstantly>{2}</summationcharacterlifeconstantly>", characterViewModel.Name, characterViewModel.Life, fullLifeOfCharacter);
                    raport += String.Format("<summationmonstername>{0}</summationmonstername><summationmonsterlifefalldown>{1}</summationmonsterlifefalldown>/<summationmonsterlifeconstantly>{2}</summationmonsterlifeconstantly></summation>", monster.Name, monster.Life, fullLifeOfMonster);
                }
            }
            while (monster.Life > 0 && characterViewModel.Life > 0);

        CharacterWin:
            if (monster.Life == 0 || monster.Life < 0)
            {
                monster.Life = 0;
                raport += String.Format("<summation><summationcharactername>{0}</summationcharactername><summationcharacterlifefalldown>{1}</summationcharacterlifefalldown>/<summationcharacterlifeconstantly>{2}</summationcharacterlifeconstantly>", characterViewModel.Name, characterViewModel.Life, fullLifeOfCharacter);
                raport += String.Format("<summationmonstername>{0}</summationmonstername><summationmonsterlifefalldown>{1}</summationmonsterlifefalldown>/<summationmonsterlifeconstantly>{2}</summationmonsterlifeconstantly></summation>", monster.Name, monster.Life, fullLifeOfMonster);
                raport += String.Format("<characterwinthebattle></characterwinthebattle>");
                characterWinner = true;
            }

        MonsterWin:
            if (characterViewModel.Life == 0 || characterViewModel.Life < 0)
            {
                characterViewModel.Life = 0;
                raport += String.Format("<summation><summationcharactername>{0}</summationcharactername><summationcharacterlifefalldown>{1}</summationcharacterlifefalldown>/<summationcharacterlifeconstantly>{2}</summationcharacterlifeconstantly>", characterViewModel.Name, characterViewModel.Life, fullLifeOfCharacter);
                raport += String.Format("<summationmonstername>{0}</summationmonstername><summationmonsterlifefalldown>{1}</summationmonsterlifefalldown>/<summationmonsterlifeconstantly>{2}</summationmonsterlifeconstantly></summation>", monster.Name, monster.Life, fullLifeOfMonster);
                raport += String.Format("<monsterwinthebattle></monsterwinthebattle>");
                characterWinner = false;
            }

            return raport;
        }
    }
}
