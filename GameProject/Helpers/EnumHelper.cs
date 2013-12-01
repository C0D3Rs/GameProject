using GameProject.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
                select new SelectListItem { Text = GetEnumDescription<TEnum>(v), Value = v.ToString() };

            return items;
        }

        public static string GetEnumDescription<TEnum>(TEnum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if ((attributes != null) && (attributes.Length > 0))
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }
    }
}
