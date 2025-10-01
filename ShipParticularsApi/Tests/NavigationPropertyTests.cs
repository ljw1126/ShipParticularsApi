using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ShipParticularsApi.Contexts;
using ShipParticularsApi.Entities;
using Xunit;
using Xunit.Abstractions;

namespace ShipParticularsApi.Tests
{
    public class NavigationPropertyTests
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
        public async Task Test2()
        {
            // Arrange
            await using (var arrangeContext = CreateContext())
            {
                List<ShipService> shipServices = [];
                shipServices.Add(new ShipService { ShipKey = "SHIP01", ServiceName = "cctv", IsCompleted = true });
                shipServices.Add(new ShipService { ShipKey = "SHIP01", ServiceName = "eu-mrv", IsCompleted = true });
                shipServices.Add(new ShipService { ShipKey = "SHIP01", ServiceName = "noon-report", IsCompleted = false });

                var shipInfo = new ShipInfo
                {
                    ShipKey = "SHIP01",
                    ShipName = "New Vessel",
                    Callsign = "CALL01",
                    ReplaceShipName = new ReplaceShipName { ShipKey = "SHIP01", ReplacedShipName = "Next Vessel" },
                    ShipServices = shipServices
                };

                arrangeContext.ShipInfos.Add(shipInfo);
                await arrangeContext.SaveChangesAsync();
            }

            // Act, Assert
            await using (var assertContext = CreateContext())
            {
                var retrevedShipInfo = await assertContext.ShipInfos
                    .Include(s => s.ReplaceShipName)
                    .Include(s => s.ShipServices)
                    .AsSplitQuery()
                    .SingleAsync(s => s.ShipKey == "SHIP01");
                var services = retrevedShipInfo.ShipServices;
                var replaceShipName = retrevedShipInfo.ReplaceShipName;

                Assert.NotNull(services);
                Assert.NotNull(replaceShipName);
            }
        }
    }
}
