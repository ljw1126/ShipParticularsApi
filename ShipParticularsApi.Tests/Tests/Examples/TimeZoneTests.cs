using Xunit.Abstractions;

namespace ShipParticularsApi.Tests.Tests.Examples
{
    public class TimeZoneTests
    {
        private readonly ITestOutputHelper _output;
        public TimeZoneTests(ITestOutputHelper output)
        {
            this._output = output;
            Environment.SetEnvironmentVariable("TZ", "UTC");
        }

        [Fact]
        public void DateTimeNow_ShouldBeUtcWhenEnvironmentSet()
        {
            // 테스트 환경이 UTC로 설정되었다고 가정하고 DateTime.Now를 확인
            // (이 검증은 로컬 시스템 시간에 따라 결과가 달라질 수 있습니다.)
            var now = DateTime.Now;
            var utcNow = DateTime.UtcNow;

            // KST와 UTC가 9시간 차이 나는지 확인 (UTC 설정이 무시될 경우)
            // 또는 0에 가까운지 확인 (UTC 설정이 성공적으로 적용될 경우)
            var difference = utcNow - now;

            // 💡 환경 변수가 성공적으로 적용되었다면 차이는 거의 0에 가까울 것입니다.
            Assert.True(Math.Abs(difference.TotalHours) < 1,
                $"DateTime.Now와 UtcNow의 차이: {difference.TotalHours} 시간. 환경 설정이 반영되지 않음.");
        }

        //// 테스트에서 현재 런타임이 인식하는 타임존 정보를 확인합니다.
        //[Fact]
        //public void CheckCurrentTimeZoneInfo()
        //{
        //    // 1. 현재 로컬 타임존 정보를 가져옵니다.
        //    var currentLocalTz = TimeZoneInfo.Local;

        //    // 2. 타임존 ID (이름)을 확인합니다.
        //    string tzId = currentLocalTz.Id;

        //    // 3. UTC와의 오프셋(시간차)을 확인합니다.
        //    TimeSpan offset = currentLocalTz.BaseUtcOffset;

        //    // 디버깅/출력 시:
        //    // TestOutputHelper를 사용하거나 Debug.WriteLine()을 사용하여 정보를 확인합니다.
        //    _output.WriteLine($"현재 타임존 ID: {tzId}, UTC 오프셋: {offset}");

        //    // 4. 검증 (Assertion) 예시
        //    // 만약 환경 변수 설정을 통해 UTC로 강제했다면, ID는 UTC이고 오프셋은 +00:00:00일 것입니다.
        //    // 환경 설정이 실패하고 KST를 따른다면, 오프셋은 +09:00:00일 것입니다.

        //    // 예를 들어, UTC로 설정되었는지 검증
        //    Assert.Equal(TimeSpan.Zero, offset);
        //    Assert.Equal("UTC", tzId);
        //}
    }
}
