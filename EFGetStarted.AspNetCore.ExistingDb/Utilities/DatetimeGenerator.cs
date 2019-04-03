using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFGetStarted.AspNetCore.ExistingDb.Utilities
{

    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime EndOfWeek(this DateTime dt, DayOfWeek endOfWeek)
        {
            int diff = (7 + (endOfWeek - dt.DayOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }

    public class DatetimeGenerator
    {
        public static DateTime GetWeekStart()
        {
             DateTime dt = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
            return dt;
        }

        public static DateTime GetWeekEnd()
        {
            DateTime dt = GetWeekStart().AddDays(4);
            return dt;
        }
    }
}
