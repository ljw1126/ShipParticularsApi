using Xunit.Abstractions;

namespace ShipParticularsApi.Tests.Tests.Testcontainers
{
    public class DatabaseContainerPerTestClass2(DatabaseFixture fixture, ITestOutputHelper output)
        : IClassFixture<DatabaseFixture>, IDisposable
    {
        public void Dispose()
        {
            output.WriteLine($"Container Id = {fixture.ContainerId}");
        }

        [Fact]
        public void SampleTest()
        {
            // Sample test implementation
            output.WriteLine("This is a sample test.");
        }
    }
}
