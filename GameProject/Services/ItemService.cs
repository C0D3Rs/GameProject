using GameProject.Enums;
using GameProject.Models;
using GameProject.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameProject.Services
{
    public class ItemService
    {
        DatabaseContext db = new DatabaseContext();

        public Item GetRandomItem(int qualityLevel)
        {
            var query = from i in db.Items
                        where i.QualityLevel <= qualityLevel
                        select i;

            int itemCount = query.Count();

            if(itemCount == 0)
            {
                return null;
            }

            Random dice = new Random();
            int record = dice.Next(0, itemCount);

            var item = query.OrderBy(i => i.Id).Skip(record).FirstOrDefault();

            return item;
        }

        public Affix GetRandomAffix(AffixType affixType, ItemType itemType, int qualityLevel)
        {
            var query = from a in db.Affixes
                        where (a.Type == affixType) && (a.QualityLevel <= qualityLevel)
                        select a;
            
            if(itemType == ItemType.Weapon)
            {
                query = from a in query
                        where a.ForWeapon == true
                        select a;
            }
            else if(itemType == ItemType.Shield)
            {
                query = from a in query
                        where a.ForShield == true
                        select a;
            }
            else if(itemType == ItemType.Armor)
            {
                query = from a in query
                        where a.ForArmor == true
                        select a;
            }
            else
            {
                query = from a in query
                        where a.ForJewelry == true
                        select a;
            }

            int itemCount = query.Count();

            if (itemCount == 0)
            {
                return null;
            }

            Random dice = new Random();
            int record = dice.Next(0, itemCount);

            var item = query.OrderBy(i => i.Id).Skip(record).FirstOrDefault();
            return item;
        }

        public GeneratedItem GetGeneratedItem(Item item, Affix prefix, Affix suffix)
        {
            if (item == null)
            {
                return null;
            }

            GeneratedItem generatedItem = new GeneratedItem()
            {
                ItemId = item.Id,
                PrimaryMinValue = item.PrimaryMinValue,
                PrimaryMaxValue = item.PrimaryMaxValue,
            };

            Random dice = new Random();

            if (prefix != null)
            {
                generatedItem.PrefixId = prefix.Id;
                generatedItem.Strength += dice.Next(prefix.MinStrength, prefix.MinStrength + 1);
                generatedItem.Dexterity += dice.Next(prefix.MinDexterity, prefix.MinDexterity + 1);
                generatedItem.Intelligence += dice.Next(prefix.MinIntelligence, prefix.MinIntelligence + 1);
                generatedItem.Vitality += dice.Next(prefix.MinVitality, prefix.MinVitality + 1);
            }

            if (suffix != null)
            {
                generatedItem.SuffixId = suffix.Id;
                generatedItem.Strength += dice.Next(suffix.MinStrength, suffix.MinStrength + 1);
                generatedItem.Dexterity += dice.Next(suffix.MinDexterity, suffix.MinDexterity + 1);
                generatedItem.Intelligence += dice.Next(suffix.MinIntelligence, suffix.MinIntelligence + 1);
                generatedItem.Vitality += dice.Next(suffix.MinVitality, suffix.MinVitality + 1);
            }

            return generatedItem;
        }
    }
}
