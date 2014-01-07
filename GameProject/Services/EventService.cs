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
    public class EventService
    {
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

        public string GetMainDescription(Event randomEvent)
        {
            return String.Format("<maindescription>{0}</maindescription>", randomEvent.MainDescription);
        }

        public bool CheckCharacterMeetsEventRequiment(Event randomEvent, CharacterViewModel characterViewModel)
        {
            int points = characterViewModel.Strength > randomEvent.RequireStrength ? randomEvent.RequireStrength : characterViewModel.Strength;
            points += characterViewModel.Dexterity > randomEvent.RequireDexterity ? randomEvent.RequireDexterity : characterViewModel.Dexterity;
            points += characterViewModel.Intelligence > randomEvent.RequireInteligence ? randomEvent.RequireInteligence : characterViewModel.Intelligence;
            points += characterViewModel.Vitality > randomEvent.RequireVitality ? randomEvent.RequireVitality : characterViewModel.Vitality;

            int requiemtPoints = randomEvent.RequireStrength + randomEvent.RequireDexterity + randomEvent.RequireInteligence + randomEvent.RequireVitality;

            if(requiemtPoints == 0)
            {
                return true;
            }

            int range = (points * 100) / requiemtPoints;

            Random dice = new Random();

            int random = dice.Next(0, 100);

            if (random <= range)
            {
                return true;
            }

            return false;
        }

        public string GetResultDescription(Event randomEvent, bool characterWinner)
        {
            if(characterWinner)
            {
                return String.Format("<successdescription>{0}</successdescription>", randomEvent.SuccessDescription);
            }
            
            return String.Format("<lostdescription>{0}</lostdescription>", randomEvent.LostDescription);
        }
    }
}
