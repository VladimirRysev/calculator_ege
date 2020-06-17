using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calculator_ege_bu.Extentions
{
    public static class DateTimeExtention
    {
        public static string ToShortDateTime(this DateTime date)
        {
            return $"{date.ToShortDateString()} {date.ToShortTimeString()}";
        }
    }
}
