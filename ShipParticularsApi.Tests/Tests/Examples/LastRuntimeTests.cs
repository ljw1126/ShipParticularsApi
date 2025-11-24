namespace ShipParticularsApi.Tests.Tests.Examples
{
    public class LastRuntimeTests
    {
        [Fact]
        public void Init()
        {
            DateTime now = DateTime.Now;
            string pid = Guid.NewGuid().ToString();

            LastRuntime actual = new LastRuntime(now, now, pid);

            actual.Equals(new LastRuntime(now, now, pid));
        }

        [Fact]
        public void WithHourDataUpdated()
        {
            DateTime now = DateTime.Now;
            string pid = Guid.NewGuid().ToString();
            LastRuntime lastRuntime = new LastRuntime(now, now, pid);

            DateTime updated = now.AddMinutes(60);

            LastRuntime actual = lastRuntime.WithHourDataUpdated(updated);

            actual.Equals(new LastRuntime(updated, now, pid));
        }

        [Fact]
        public void WithDayDataUpdated()
        {
            DateTime now = DateTime.Now;
            string pid = Guid.NewGuid().ToString();
            LastRuntime lastRuntime = new LastRuntime(now, now, pid);

            DateTime updated = now.AddDays(1);

            LastRuntime actual = lastRuntime.WithDayDataUpdated(updated);

            actual.Equals(new LastRuntime(now, updated, pid));
        }

        [Fact]
        public void IsHourIntervalMetWhenTrue()
        {
            var lastRun = new DateTime(2025, 1, 1, 10, 0, 0);
            var vo = new LastRuntime(lastRun, lastRun, "hostPID");

            var now = lastRun.AddMinutes(12);

            bool actual = vo.IsHourIntervalMet(now);

            actual.Equals(true);
        }

        [Fact]
        public void IsHourIntervalMetWhenFalse()
        {
            var lastRun = new DateTime(2025, 1, 1, 10, 0, 0);
            var vo = new LastRuntime(lastRun, lastRun, "hostPID");

            bool actual = vo.IsHourIntervalMet(lastRun.AddMinutes(11));

            actual.Equals(false);
        }

        [Fact]
        public void IsDayIntervalMetWhenTrue()
        {
            var lastRun = new DateTime(2025, 1, 1, 10, 0, 0);
            var vo = new LastRuntime(lastRun, lastRun, "host");

            bool actual = vo.IsDayIntervalMet(lastRun.AddMinutes(30));

            actual.Equals(true);
        }

        [Fact]
        public void IsDayIntervalMetWhenFalse()
        {
            var lastRun = new DateTime(2025, 1, 1, 10, 0, 0);
            var vo = new LastRuntime(lastRun, lastRun, "host");

            bool actual = vo.IsDayIntervalMet(lastRun.AddMinutes(29));

            actual.Equals(false);
        }
    }
}
