using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PH.DateTimeUtils.Tests
{
    public class WeekTest
    {
        [Fact]
        public void FormatString()
        {
            var first2023 = new Week(1, 2023);

            var s  = first2023.ToString("s");
            var l  = first2023.ToString("S");
            var i  = first2023.ToString("i");
            var lo = first2023.ToString("I");

            Assert.Equal("1-2023", s);
            Assert.Equal("2023-1", i);
            Assert.Equal("2023-01", lo);
            Assert.Equal("01-2023 (2023-01-02 ~ 2023-01-08)", l);
        }

        [Fact]
        public void InvalidDataThrowException()
        {
            Exception? e0 = null;
            Exception? e1 = null;
            

            try
            {
                var a = new Week(0, 1);
            }
            catch (Exception e)
            {
                e0 = e;
            }

            try
            {
                var b = new Week(1, 0);
            }
            catch (Exception e)
            {
                e1 = e;
            }

            

            Assert.NotNull(e0);
            Assert.NotNull(e1);
        }

        [Fact]
        public void CurrentWeekIsOnCurrentDateTimeYear()
        {
            var current = Week.Current;

            Assert.True(current.Year == DateTime.UtcNow.Year);

        }

        [Fact]
        public void Order()
        {
            var r = new Week[] { new Week(53, 2020), new Week(1, 2020), new Week(1, 2019) };
            var l = r.OrderBy(x => x).ToArray();


            Assert.Equal( l[0] , r[2] );
            Assert.Equal( l[1] , r[1] );
            Assert.Equal( l[2] , r[0] );
        }

        [Fact]
        public void TestingDaysForWeek()
        {
            var d0    = new DateTime(2020, 12, 30);
            var week0 = Week.FromDateTime(d0);

            var d1    = new DateTime(2021, 1, 1);
            var week1 = Week.FromDateTime(d1);

            var week3 = new Week(53, 2020);

            var startOn = new DateTime(2020, 12, 28, 0, 0, 0, DateTimeKind.Utc);
            var endOnd  = startOn.AddDays(7).AddMilliseconds(-1);

            bool eq0 = week0.Equals(week1);
            bool eq1 = week1 == week3;

            Week? w   = new Week(2, 2020);
            bool  eq3 = w != null;

            object b = Week.Current;




            var minus    = w < week0;
            var minOrEq  = w <= week0;
            var max      = w > week0;
            var maxOrEq = w >= week0;

            var diff = w.Equals(b);

            Assert.True(eq0);
            Assert.True(eq1);
            Assert.True(eq3);

            Assert.False(diff);

            Assert.True(minus);
            Assert.True(minOrEq);
            Assert.False(max);
            Assert.False(maxOrEq);

            Assert.Equal(53, week0.Number);
            Assert.Equal(week0, week1);
            Assert.Equal(week0, week3);

            Assert.Equal(startOn, week0.Start);
            Assert.Equal(startOn, week1.Start);
            Assert.Equal(startOn, week3.Start);

            Assert.Equal(endOnd, week0.End);
            Assert.Equal(endOnd, week1.End);
            Assert.Equal(endOnd, week3.End);



        }

    }
}