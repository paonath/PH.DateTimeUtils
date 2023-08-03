namespace PH.DateTimeUtils.Tests
{
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

        [Fact]
        public void RuntimeException()
        {
            Exception e = null;
            try
            {
                var r = PH.DateTimeUtils.Weeks.GetStartAndEndDateTimeFromWeekNumber(2023, 53);
            }
            catch (Exception exception)
            {
                e = exception;
                Assert.True(e is ArgumentOutOfRangeException outOfRangeException);
                
            }
            Assert.NotNull(e);
        }
    }


}