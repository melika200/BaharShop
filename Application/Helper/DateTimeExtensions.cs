using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaharShop.Application.Helper
{
    public static class DateTimeExtensions
    {
        public static string ToPersianDateString(this DateTime dateTime)
        {
            PersianCalendar persianCalendar = new PersianCalendar();

            int year = persianCalendar.GetYear(dateTime);
            int month = persianCalendar.GetMonth(dateTime);
            int day = persianCalendar.GetDayOfMonth(dateTime);

            return $"{year:0000}/{month:00}/{day:00}";
        }

        public static string ToPersianDateString(this DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                return string.Empty;
            }

            return dateTime.Value.ToPersianDateString();
        }
    }
}
