using System;
using System.Globalization;

namespace PH.DateTimeUtils
{
    /// <summary>
    /// Week Related Utility
    /// </summary>
    public class Weeks
    {
        /// <summary>
        /// Gets the <see cref="Week"/>  from week number.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="weekNumber">The week number.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static Week GetWeekFromWeekNumber(int year, int weekNumber, CultureInfo culture = null)
        {
            var r = GetStartAndEndDateTimeFromWeekNumber(year, weekNumber, culture);
            return new Week(weekNumber, year, r.Start, r.End);
        }

        /// <summary>
        /// Gets the start and end date time from week number (Both <see cref="DateTime">dates</see> are UTC).
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="weekNumber">The week number.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static (DateTime Start, DateTime End) GetStartAndEndDateTimeFromWeekNumber(int year, int weekNumber, CultureInfo culture = null)
        {
            if (null == culture)
            {
                culture = CultureInfo.CurrentCulture;
            }

            var dfi      = culture.DateTimeFormat;
            var cal      = dfi.Calendar;
            var firstDay = new DateTime(year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            
            var firstweek = cal.GetWeekOfYear(firstDay, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
            while (firstweek != 1)
            {
                firstDay  = firstDay.AddDays(1);
                firstweek = cal.GetWeekOfYear(firstDay, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
            }

            while (firstweek < weekNumber)
            {
                firstDay  = firstDay.AddDays(1);
                firstweek = cal.GetWeekOfYear(firstDay, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
            }

            var lastDay = firstDay.AddDays(1);

            while (firstweek < weekNumber + 1)
            {
                lastDay   = lastDay.AddDays(1);
                firstweek = cal.GetWeekOfYear(lastDay, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
            }

            lastDay = lastDay.AddMilliseconds(-1);


            return (firstDay, lastDay);
        }
    }
}
