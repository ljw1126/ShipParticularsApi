using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ShipParticularsApi.Contexts;
using ShipParticularsApi.Entities;
using Xunit;
using Xunit.Abstractions;

using static ShipParticularsApi.Tests.Builders.ShipInfoTestBuilder;

namespace ShipParticularsApi.Tests
{
    public class NavigationPropertyTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<ShipParticularsContext> _options;
        private readonly ITestOutputHelper _output;

        // NOTE: beforeEach
        public NavigationPropertyTests(ITestOutputHelper output)
        {
            _output = output;

            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            _options = new DbContextOptionsBuilder<ShipParticularsContext>()
                .UseSqlite(_connection)
                .UseLazyLoadingProxies()
                .LogTo(message => _output.WriteLine(message), LogLevel.Information)
                .EnableSensitiveDataLogging()
                .Options;

            var context = new ShipParticularsContext(_options);
            context.Database.EnsureCreated();
        }

        // NOTE: AfterEach
        public void Dispose() => _connection.Dispose();

        ShipParticularsContext CreateContext() => new(_options);

        [Fact]
        public async Task SplitQueryTest()
        {
            // Arrange
            await using (var arrangeContext = CreateContext())
            {
                ShipService cctv = new() { ServiceName = "cctv", IsCompleted = true };
                ShipService euMrv = new() { ServiceName = "eu-mrv", IsCompleted = true };
                ShipService noonReport = new() { ServiceName = "noon-report", IsCompleted = false };

                ReplaceShipName replaceShipName = new() { ReplacedShipName = "Next Vessel" };

                arrangeContext.ShipInfos.Add(ShipInfo()
                    .WithShipKey("SHIP01")
                    .WithReplaceShipName(replaceShipName)
                    .WithShipServices(cctv, euMrv, noonReport)
                    .Build());
                await arrangeContext.SaveChangesAsync();
            }

            // Act, Assert
            await using (var assertContext = CreateContext())
            {
                var savedShipInfo = await assertContext.ShipInfos
                    .Include(s => s.ReplaceShipName)
                    .Include(s => s.ShipServices)
                    .AsSplitQuery()
                    .SingleAsync(s => s.ShipKey == "SHIP01");

                Assert.NotNull(savedShipInfo.ShipServices);
                Assert.NotNull(savedShipInfo.ReplaceShipName);
            }
        }
    }
}
