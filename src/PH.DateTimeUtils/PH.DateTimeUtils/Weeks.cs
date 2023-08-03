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
        /// Gets the week of the Date.
        ///
        /// <para>
        ///This method is just a wrapper for
        /// <code>
        /// if (null == culture)
        /// {
        ///     culture = CultureInfo.CurrentCulture;
        /// }
        /// 
        /// var dfi = culture.DateTimeFormat;
        /// 
        /// var cal = dfi.Calendar;
        /// return cal.GetWeekOfYear(date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        /// </code>
        /// </para>
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>integer Week number</returns>
        public static int GetWeek(DateTime date, CultureInfo culture = null)
        {
            if (null == culture)
            {
                culture = CultureInfo.CurrentCulture;
            }

            var dfi = culture.DateTimeFormat;
            var cal = dfi.Calendar;

            return cal.GetWeekOfYear(date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        }

        /// <summary>
        /// Gets the <see cref="Week"/>  from week number.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="weekNumber">The week number.</param>
        /// <param name="culture">The culture.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">weekNumber - The year {year} does not have {weekNumber} weeks</exception>
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
        /// <exception cref="System.ArgumentOutOfRangeException">weekNumber - The year {year} does not have {weekNumber} weeks</exception>
        /// <returns></returns>
        public static (DateTime Start, DateTime End) GetStartAndEndDateTimeFromWeekNumber(int year, int weekNumber, CultureInfo culture = null)
        {
            var res              = GetStartAndEndDateTimeFromWeekNumberWithNoThroException(year, weekNumber, true, culture);
            if (!res.Start.HasValue || !res.End.HasValue)
            {
                throw new ArgumentOutOfRangeException(nameof(weekNumber), weekNumber,
                                                      $"The year {year} does not have {weekNumber} weeks");
            }

            return (res.Start.Value, res.End.Value);
        }


        internal static (DateTime? Start, DateTime? End) GetStartAndEndDateTimeFromWeekNumberWithNoThroException(
            int year, int weekNumber, bool exitOnChangeYear = true, CultureInfo culture = null)
        {
            if (null == culture)
            {
                culture = CultureInfo.CurrentCulture;
            }

            var dfi       = culture.DateTimeFormat;
            var firstDay  = new DateTime(year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var firstweek = GetWeek(firstDay, culture);

            while (firstweek != 1)
            {
                firstDay  = firstDay.AddDays(1);
                firstweek = GetWeek(firstDay, culture);
            }


            while (firstweek < weekNumber)
            {
                int daysToAdd = 1;
                if (firstDay.DayOfWeek == dfi.FirstDayOfWeek)
                {
                    daysToAdd = 7;
                }

                firstDay = firstDay.AddDays(daysToAdd);
                if (exitOnChangeYear && firstDay.Year != year)
                {
                    return (null,null);
                }

                firstweek = GetWeek(firstDay, culture);
            }

            var lastDay = firstDay.AddDays(7).AddMilliseconds(-1);



            return (firstDay, lastDay);
        }

        
    }
}
