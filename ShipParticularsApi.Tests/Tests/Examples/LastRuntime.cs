namespace ShipParticularsApi.Tests.Tests.Examples
{
    public class LastRuntime
    {
        private const double RequiredMinutesForHourData = 0.2; // 12 Minutes
        private const double RequiredMinutesForDayData = 0.5; // 30 Minutes

        public DateTime HourLastRuntime { get; }
        public DateTime DayLastRuntime { get; }
        public string HostId { get; }

        public LastRuntime(DateTime hourLastRuntime, DateTime dayLastRuntime, string hostId)
        {
            HourLastRuntime = hourLastRuntime;
            DayLastRuntime = dayLastRuntime;
            HostId = hostId;
        }

        public LastRuntime WithHourDataUpdated(DateTime now)
        {
            return new LastRuntime(now, DayLastRuntime, HostId);
        }

        public LastRuntime WithDayDataUpdated(DateTime now)
        {
            return new LastRuntime(HourLastRuntime, now, HostId);
        }

        public bool IsHourIntervalMet(DateTime now)
        {
            var diffHourTime = now - HourLastRuntime;
            return diffHourTime.TotalHours >= RequiredMinutesForHourData;
        }

        public bool IsDayIntervalMet(DateTime now)
        {
            var diffDayTime = now - DayLastRuntime;
            return diffDayTime.TotalHours >= RequiredMinutesForDayData;
        }
    }
}
