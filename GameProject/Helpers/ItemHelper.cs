using GameProject.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameProject.Helpers
{
    public class ItemHelper
    {
        public static string GetCorrectPrefixName(string prefixName, string itemName)
        {
            string[] words = itemName.Split(' ');

            if (prefixName == null)
            {
                return null;
            }
            if (words[0].EndsWith("o") && prefixName.EndsWith("ki") ||
                (words[0].EndsWith("o") && prefixName.EndsWith("i"))
                )
            {
                prefixName = prefixName.Remove(prefixName.Length - 1) + "ie";
            }
            else if ((words[0].EndsWith("o") && prefixName.EndsWith("ny")) ||
                (words[0].EndsWith("o") && prefixName.EndsWith("ty")) ||
                (words[0].EndsWith("o") && prefixName.EndsWith("cy")) ||
                (words[0].EndsWith("o") && prefixName.EndsWith("wy")) ||
                (words[0].EndsWith("o") && prefixName.EndsWith("ły")) ||
                (words[0].EndsWith("e")) ||
                (words[0].EndsWith("ja")) ||
                (words[0].EndsWith("i"))
                )
            {
                prefixName = prefixName.Remove(prefixName.Length - 1) + "e";
            }
            else if (words[0].EndsWith("a") && prefixName.EndsWith("y") ||
                (words[0].EndsWith("a") && prefixName.EndsWith("i"))
                )
            {
                prefixName = prefixName.Remove(prefixName.Length - 1) + "a";
            }
            else if (words[0].EndsWith("o") && prefixName.EndsWith("ki"))
            {
                prefixName = prefixName + "e";
            }
            return prefixName;
        }

        public static string GetFullName(string itemName = "", string prefixName = "", string suffixName = "")
        {
            return string.Format("{1} {0} {2}", itemName, GetCorrectPrefixName(prefixName, itemName), suffixName);
        }

        public static int GetCalculatedPrice(int itemPrice, int prefixPrice, int suffixPrice, int itemDurability, int itemDurabilityMax)
        {
            decimal totalPrice = (itemPrice + prefixPrice + suffixPrice) * ((decimal)itemDurability / (decimal)itemDurabilityMax);
            return (int)Math.Ceiling(totalPrice);
        }
    }
}
