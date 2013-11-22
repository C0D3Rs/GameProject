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
            var splitItem = itemName.Split(' ');
            string[] words = splitItem;

            if (words[0].EndsWith("o") && prefixName.EndsWith("ki"))
            {
                prefixName = prefixName.Remove(prefixName.Length - 1) + "ie";
            }
            else if (words[0].EndsWith("o") && prefixName.EndsWith("ny"))
            {
                prefixName = prefixName.Remove(prefixName.Length - 1) + "e";
            }
            else if (words[0].EndsWith("o") && prefixName.EndsWith("ty"))
            {
                prefixName = prefixName.Remove(prefixName.Length - 1) + "e";
            }
            else if (words[0].EndsWith("o") && prefixName.EndsWith("cy"))
            {
                prefixName = prefixName.Remove(prefixName.Length - 1) + "e";
            }
            else if (words[0].EndsWith("o") && prefixName.EndsWith("wy"))
            {
                prefixName = prefixName.Remove(prefixName.Length - 1) + "e";
            }
            else if (words[0].EndsWith("o") && prefixName.EndsWith("ły"))
            {
                prefixName = prefixName.Remove(prefixName.Length - 1) + "e";
            }
            else if (words[0].EndsWith("e"))
            {
                prefixName = prefixName.Remove(prefixName.Length - 1) + "e";
            }
            else if (words[0].EndsWith("ja"))
            {
                prefixName = prefixName.Remove(prefixName.Length - 1) + "e";
            }
            else if (words[0].EndsWith("i"))
            {
                prefixName = prefixName.Remove(prefixName.Length - 1) + "e";
            }
            else if (words[0].EndsWith("o") && prefixName.EndsWith("i"))
            {
                prefixName = prefixName.Remove(prefixName.Length - 1) + "ie";
            }
            else if (words[0].EndsWith("a") && prefixName.EndsWith("y"))
            {
                prefixName = prefixName.Remove(prefixName.Length - 1) + "a";
            }
            else if (words[0].EndsWith("a") && prefixName.EndsWith("r"))
            {
                return prefixName;
            }
            else if (words[0].EndsWith("o") && prefixName.EndsWith("r"))
            {
                return prefixName;
            }
            else if (words[0].EndsWith("o") && prefixName.EndsWith("ki"))
            {
                prefixName = prefixName + "e";
            }
            else if (words[0].EndsWith("a") && prefixName.EndsWith("y"))
            {
                prefixName = prefixName.Remove(prefixName.Length - 1) + "a";
            }
            else if (words[0].EndsWith("a") && prefixName.EndsWith("i"))
            {
                prefixName = prefixName.Remove(prefixName.Length - 1) + "a";
            }
            return prefixName;
        }
    }
}
