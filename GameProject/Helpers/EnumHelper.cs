using GameProject.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameProject.Helpers
{
    public static class EnumHelper
    {
        public static IEnumerable<SelectListItem> GetSelectList<TEnum>()
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new InvalidOperationException("Przekazana klasa nie jest typu enum.");
            }

            IEnumerable<TEnum> values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
            
            IEnumerable<SelectListItem> items = 
                from v in values
                select new SelectListItem { Text = v.ToString(), Value = v.ToString() };

            return items;
        }
    }
}
