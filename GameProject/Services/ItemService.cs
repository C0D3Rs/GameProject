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
    }
}
