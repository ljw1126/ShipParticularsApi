using System.Globalization;
using Xunit.Abstractions;

namespace ShipParticularsApi.Tests.Tests.Examples
{
    public class DateTimeTests
    {
        private readonly ITestOutputHelper _output;

        public DateTimeTests(ITestOutputHelper output)
        {
            _output = output;
        }


        // logger10min QueueTrigger에서 아래 값을 가져와 파일 필터링을 한다
        [Fact]
        public void ConvertDynamicToDateTimeTest()
        {
            string lastDateTimeFirstInsert10Min = "2025-11-19T09:00:00Z";

            DateTime actual = ConvertDynamicToDateTime(lastDateTimeFirstInsert10Min);

            _output.WriteLine(actual.ToString()); // 2025-11-19 오전 9:00:00
        }


        public static DateTime ConvertDynamicToDateTime(dynamic dateTime)
        {
            string utcString = Convert.ToString(dateTime, CultureInfo.InvariantCulture);

            if (utcString == null)
            {
                return new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            }

            DateTime dateConverted = Convert.ToDateTime(utcString, CultureInfo.InvariantCulture);

            if (utcString.Last() == 'Z' || utcString.Last() == 'z')
            {
                dateConverted = dateConverted.ToUniversalTime();
            }

            return dateConverted;
        }

        [Fact]
        public void ConvertToDateTime()
        {
            string firstDateTime = "2025-11-18  12:00:20 AM";
            string lastDateTime = "2025-11-18  12:06:20 AM";

            DateTime firstDate = Convert.ToDateTime(firstDateTime);
            DateTime lastDate = Convert.ToDateTime(lastDateTime);

            _output.WriteLine($"{firstDateTime} : {firstDate}"); // 2025-11-18  12:00:20 AM : 2025-11-18 오전 12:00:20
            _output.WriteLine($"{lastDateTime} : {lastDate}"); //  2025-11-18  12:06:20 AM : 2025-11-18 오전 12:06:20
        }

        [Fact]
        public void ConvertTest()
        {
            string lastDateTime = "2025-11-18  12:06:20 AM";

            DateTime lastTimeStamp = Convert.ToDateTime(lastDateTime);
            lastTimeStamp = lastTimeStamp.AddMinutes(10);
            var minTemp = lastTimeStamp.Minute / 10 * 10;
            lastTimeStamp = new DateTime(lastTimeStamp.Year, lastTimeStamp.Month, lastTimeStamp.Day, lastTimeStamp.Hour, minTemp, 0);

            string actual = lastTimeStamp.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); //  2025-11-18  12:06:20 AM : 2025-11-18T00:10:00.000Z

            _output.WriteLine($"{lastDateTime} : {actual}");
        }

        [Fact]
        public void DateTimeTest()
        {
            DateTime now = DateTime.Now;
            DateTime utcNow = DateTime.UtcNow;

            _output.WriteLine($"DateTime.Now: {now}"); // 2025-11-20 오전 7:35:33
            _output.WriteLine($"DateTime.UtcNow: {utcNow}"); // 2025-11-20 오전 7:35:33
        }

        // 윈도우의 TIME_ZONE이 KST(한국 표준시)로 설정되어 있을 때   
        [Fact]
        public void ConvertToDateTimeTest()
        {
            string lastDateTimeFirstInsert10Min = "2025-11-19T09:00:00Z";

            //var actual = Convert.ToDateTime(lastDateTimeFirstInsert10Min);
            var actual = DateTime.Parse(lastDateTimeFirstInsert10Min, CultureInfo.InvariantCulture); // 2025-11-19 오후 6:00:00
            actual = actual.ToUniversalTime(); //  2025-11-19 오전 9:00:00


            _output.WriteLine(actual.ToString()); //2025-11-19 오후 6:00:00
        }
    }
}