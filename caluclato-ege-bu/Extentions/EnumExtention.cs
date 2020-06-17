using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Calculator_ege_bu.Extentions
{
    public static class EnumExtention
    {
		public static string GetDisplayName(this Enum enumValue)
		{
			return (enumValue.GetType()
							.GetMember(enumValue.ToString())
							.First()
							?.GetCustomAttribute<DisplayAttribute>()
							?.GetName()) ?? enumValue.ToString();
		}

		public static IEnumerable<T> GetValues<T>() where T : struct, IConvertible
		{
			return Enum.GetValues(typeof(T)).Cast<T>();
		}

		public static T GetEnumFromString<T>(this string name) where T : Enum
        {
            foreach (T item in Enum.GetValues(typeof(T)))
            {
                if (item.GetDisplayName() == name)
                {
					return item;
                }
            }
			return (T)Enum.GetValues(typeof(T)).GetValue(0);
        }
    }
}
