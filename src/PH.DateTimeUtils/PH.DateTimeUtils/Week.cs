using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace PH.DateTimeUtils
{
    /// <summary>
    /// Represents a week expressed as a <see cref="Number">Number</see> and <see cref="Start">Start</see> and <see cref="End">End</see> date times.
    /// </summary>
    public readonly struct Week : IEquatable<Week> , IComparable<Week> 
    {
        private readonly int      _week;
        private readonly int      _year;
        private readonly DateTime _utcStart;
        private readonly DateTime _utcEnd;

        public int      Year   => _year;
        public int      Number => _week;
        public DateTime Start  => _utcStart;
        public DateTime End    => _utcEnd;

        public Week(int week, int year, DateTime utcStart, DateTime utcEnd)
        {
            if (week <= 0 || week > 53)
            {
                throw new ArgumentOutOfRangeException(nameof(week), "Provide value between 1 and 53");
            }

            if (year <= 1 || year > 9999)
            {
                throw new ArgumentOutOfRangeException(nameof(year), "Provide value between 1 and 9999");
            }
            _week     = week;
            _year     = year;
            _utcStart = utcStart;
            _utcEnd   = utcEnd;
        }

        public Week(int week, int year, CultureInfo culture = null)
        {
            if (week <= 0 || week > 53)
            {
                throw new ArgumentOutOfRangeException(nameof(week), "Provide value between 1 and 53");
            }

            if (year <= 1 || year > 9999)
            {
                throw new ArgumentOutOfRangeException(nameof(year), "Provide value between 1 and 9999");
            }

            _week = week;
            _year = year;
            var computed = Weeks.GetStartAndEndDateTimeFromWeekNumber(year, week, culture);
            _utcStart = computed.Start;
            _utcEnd   = computed.End;
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(Week other) => _week == other._week && _year == other._year;

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>true if <paramref name="obj">obj</paramref> and this instance are the same type and represent the same value; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is Week other && Equals(other);

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();

        }

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>The fully qualified type name.</returns>
        public override string ToString() => $"Week {_week}/{_year} - From '{_utcStart:yyyy-MM-dd}' To '{_utcEnd:yyyy-MM-dd}'";

        

        

        /// <summary>
        /// Gets the current <see cref="Week"/>.
        /// </summary>
        /// <value>
        /// The current.
        /// </value>
        public static  Week Current =>  Week.FromDateTime(DateTime.UtcNow, CultureInfo.CurrentCulture);

        /// <summary>
        /// Get <see cref="Week"/> from <see cref="DateTime"/>.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static Week FromDateTime(DateTime dateTime, CultureInfo culture = null)
        {
            if (null == culture)
            {
                culture = CultureInfo.CurrentCulture;
            }

            var dfi = culture.DateTimeFormat;
            var cal = dfi.Calendar;
            

            var weekNo = cal.GetWeekOfYear(dateTime, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
            var week   = Weeks.GetWeekFromWeekNumber(dateTime.Year, weekNo);
            return week;
        }

        public int CompareTo(Week other)
        {
            var weekComparison = _week.CompareTo(other._week);
            if (weekComparison != 0)
            {
                return weekComparison;
            }

            return _year.CompareTo(other._year);
        }
    }
}