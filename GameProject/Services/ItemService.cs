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

        private string GetCorrectPrefixName(string prefixName, string itemName)
        {
            if (itemName.EndsWith("a") && prefixName.EndsWith("r"))
            {
                return prefixName;
            }
            else if (itemName.EndsWith("a"))
            {
                prefixName = prefixName.Remove(prefixName.Length - 1) + "a";
            }

            return prefixName;
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
                Status = ItemStatus.Bagpack,
                Type = item.Type,
                SubType = item.SubType,
                Name = item.Name,
                Durability = item.Durability,
                Price = item.Price,
                PrimaryMinValue = item.PrimaryMinValue,
                PrimaryMaxValue = item.PrimaryMaxValue,
                RequireStrength = item.RequireStrength,
                QualityLevel = item.QualityLevel
            };

            Random dice = new Random();

            if (prefix != null)
            {
                generatedItem.Strength = dice.Next(prefix.MinStrength, prefix.MinStrength + 1);
                generatedItem.Dexterity = dice.Next(prefix.MinDexterity, prefix.MinDexterity + 1);
                generatedItem.Intelligence = dice.Next(prefix.MinIntelligence, prefix.MinIntelligence + 1);
                generatedItem.Vitality = dice.Next(prefix.MinVitality, prefix.MinVitality + 1);
                generatedItem.Price += prefix.Price;
                generatedItem.Name = String.Format("{0} {1}", GetCorrectPrefixName(prefix.Name, generatedItem.Name), generatedItem.Name);
            }

            if (suffix != null)
            {
                generatedItem.Strength = dice.Next(suffix.MinStrength, suffix.MinStrength + 1);
                generatedItem.Dexterity = dice.Next(suffix.MinDexterity, suffix.MinDexterity + 1);
                generatedItem.Intelligence = dice.Next(suffix.MinIntelligence, suffix.MinIntelligence + 1);
                generatedItem.Vitality = dice.Next(suffix.MinVitality, suffix.MinVitality + 1);
                generatedItem.Price += suffix.Price;
                generatedItem.Name = String.Format("{0} {1}", generatedItem.Name, suffix.Name);
            }

            return generatedItem;
        }
    }
}
