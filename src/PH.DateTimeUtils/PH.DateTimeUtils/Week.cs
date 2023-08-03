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

        /// <summary>
        /// Initializes a new instance of the <see cref="Week"/> struct.
        /// </summary>
        /// <param name="week">The week.</param>
        /// <param name="year">The year.</param>
        /// <param name="utcStart">The UTC start.</param>
        /// <param name="utcEnd">The UTC end.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// week - Provide value between 1 and 53
        /// or
        /// year - Provide value between 1 and 9999
        /// </exception>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="Week"/> struct.
        /// </summary>
        /// <param name="week">The week.</param>
        /// <param name="year">The year.</param>
        /// <param name="culture">The culture.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// week - Provide value between 1 and 53
        /// or
        /// year - Provide value between 1 and 9999
        /// or
        /// week - The year {year} does not have {weekNumber} weeks
        /// </exception>
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
        public bool Equals(Week other) => _week == other._week && _year == other._year && _utcStart == other._utcStart && _utcEnd == other._utcEnd;

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

        /// <summary>Returns the string representation of this instance.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString() => ToString("F");


        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(string format)
        {
            var w = $"{_week}".PadLeft(2, '0');

            if (format == "F")
            {
                return $"Week {_week}-{_year} - From '{_utcStart:yyyy-MM-dd}' To '{_utcEnd:yyyy-MM-dd}'";
            }

            if (format == "S")
            {
                
                return $"{w}-{_year} ({_utcStart:yyyy-MM-dd} ~ {_utcEnd:yyyy-MM-dd})";
            }

            if (format == "s")
            {
                return $"{_week}-{_year}";
            }

            if (format == "i")
            {
                return $"{_year}-{_week}";
            }

            if (format == "I")
            {
                return $"{_year}-{w}";
            }

            return ToString();
        }



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
            var weekNumber = Weeks.GetWeek(dateTime, culture);
            int tryYear    = dateTime.Year;
            
            

            var res = Weeks.GetStartAndEndDateTimeFromWeekNumberWithNoThroException(dateTime.Year, weekNumber, true, culture);
            if (res.Start.HasValue && res.End.HasValue)
            {
                return new Week(weekNumber, dateTime.Year, res.Start.GetValueOrDefault(),
                                res.End.GetValueOrDefault());

            }

            int minYear = tryYear - 1;
            res = Weeks.GetStartAndEndDateTimeFromWeekNumberWithNoThroException(minYear, weekNumber, true, culture);
            if (res.Start.HasValue && res.End.HasValue)
            {
                return new Week(weekNumber, minYear, res.Start.GetValueOrDefault(),
                                res.End.GetValueOrDefault());
            }

            int maxYear = tryYear + 1;
            res = Weeks.GetStartAndEndDateTimeFromWeekNumberWithNoThroException(maxYear, weekNumber, true,
                                                                                culture);
            return new Week(weekNumber, maxYear, res.Start.GetValueOrDefault(), res.End.GetValueOrDefault());


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

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Week t1, Week t2) => t1.Equals(t2);


        public static bool operator !=(Week t1, Week t2) => !t1.Equals(t2);
        
        
        public static bool operator <(Week t1, Week t2)
        {
            if (t1.Year < t2.Year)
            {
                return true;
            }

            if (t1.Year > t2.Year)
            {
                return false;
            }

            if (t1.Number < t2.Number)
            {
                return true;
            }

            return false;
           

        }

        public static bool operator >(Week t1, Week t2)
        {
            if (t1.Year > t2.Year)
            {
                return true;
            }

            if (t1.Year < t2.Year)
            {
                return false;
            }

            if (t1.Number > t2.Number)
            {
                return true;
            }

            return false;

        }

        public static bool operator <=(Week t1, Week t2)
        {
            if (t1 == t2)
            {
                return true;
            }

            return t1 < t2;
        }

        public static bool operator >=(Week t1, Week t2)
        {
            if (t1 == t2)
            {
                return true;
            }

            return t1 > t2;
        }
    }
}