namespace PH.DateTimeUtils.Tests
{

    public class WeekTest
    {
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
    }

    public class WeeksTests
    {
        [Fact]
        public void GetStartAndEndDateTimeFromWeekNumber()
        {
            var number = 31;

            var result = PH.DateTimeUtils.Weeks.GetStartAndEndDateTimeFromWeekNumber(2023, number);

            Assert.Equal(new DateTime(2023,7,31,0,0,0, DateTimeKind.Utc), result.Start);

            var end = new DateTime(2023, 8, 6, 23, 59, 59, DateTimeKind.Utc).AddMilliseconds(999);
            Assert.Equal(end, result.End);

            

        }

        [Fact]
        public void GetWeekFromWeekNumber()
        {
            var weekNo       = 31;
            var result       = PH.DateTimeUtils.Weeks.GetWeekFromWeekNumber(2023, weekNo);


            var end          = new DateTime(2023, 8, 6, 23, 59, 59, DateTimeKind.Utc).AddMilliseconds(999);
            var expectedWeek = new Week(weekNo, 2023, new DateTime(2023, 7, 31, 0, 0, 0, DateTimeKind.Utc), end);

            Assert.Equal(expectedWeek, result);
        }
    }


}