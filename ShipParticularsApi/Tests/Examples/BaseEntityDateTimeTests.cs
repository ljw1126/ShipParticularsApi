using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ShipParticularsApi.Contexts;
using ShipParticularsApi.ValueObjects;
using Xunit;
using Xunit.Abstractions;
using static ShipParticularsApi.Tests.Builders.Entities.ShipInfoTestBuilder;

namespace ShipParticularsApi.Tests.Examples
{
    public class BaseEntityDateTimeTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<ShipParticularsContext> _options;
        private readonly ITestOutputHelper _output;
        private const string FixedUserId = "TEST_USER_01";

        // NOTE: beforeEach
        public BaseEntityDateTimeTests(ITestOutputHelper output)
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
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        // NOTE: AfterEach
        public void Dispose() => _connection.Dispose();

        ShipParticularsContext CreateContext() => new(_options);

        // NOTE. UpdateDateTime이 nullable인데도 DateTime 기본값 할당됨
        // Did not expect actual.ShipSatellite.UpdateDateTime to have a value, but found <0001-01-01 00:00:00.000>.
        [Fact]
        public async Task CreateDateTime_test()
        {
            // Arrange
            const string shipKey = "UNIQUE_SHIP_KEY";

            var startTime = DateTime.UtcNow;

            await using (var arrangeContext = CreateContext())
            {
                arrangeContext.ShipInfos.Add(UsingKtSat(shipKey, FixedUserId, 1L).Build());
                await arrangeContext.SaveChangesAsync();
            }

            var endTime = DateTime.UtcNow;

            // Act, Assert
            await using (var assertContext = CreateContext())
            {
                var actual = await assertContext.ShipInfos
                    .AsNoTracking()
                    .Include(s => s.ShipSatellite)
                    .SingleOrDefaultAsync(s => s.ShipKey == shipKey);

                actual.Should().NotBeNull();
                actual.ShipSatellite.Should().NotBeNull();
                actual.ShipSatellite.CreateDateTime.Should().BeOnOrAfter(startTime)
                    .And.BeOnOrBefore(endTime);
                actual.ShipSatellite.UpdateDateTime.Should().BeNull();
            }
        }

        [Fact]
        public async Task UpdateDateTime_test()
        {
            // Arrange
            var startTime = DateTime.UtcNow;
            const string shipKey = "UNIQUE_SHIP_KEY";

            await using (var arrangeContext = CreateContext())
            {
                arrangeContext.ShipInfos.Add(UsingKtSat(shipKey, FixedUserId, 1L).Build());
                await arrangeContext.SaveChangesAsync();
            }

            // Act
            await using (var actConext = CreateContext())
            {
                var shipInfo = await actConext.ShipInfos
                    .Include(s => s.ShipSatellite)
                    .Include(s => s.ShipServices)
                    .Include(s => s.SkTelinkCompanyShip)
                    .AsSplitQuery()
                    .SingleAsync(s => s.ShipKey == shipKey && s.IsService == true);

                shipInfo.ActiveGpsService(new SatelliteDetails("SATELLITE_ID", "SK_TELINK", "COMPANY_NAME"), FixedUserId);
                await actConext.SaveChangesAsync();
            }

            var endTime = DateTime.UtcNow;

            // Assert
            await using (var assertContext = CreateContext())
            {
                var actual = await assertContext.ShipInfos
                    .AsNoTracking()
                    .Include(s => s.ShipSatellite)
                    .SingleAsync(s => s.ShipKey == shipKey && s.IsService == true);

                actual.Should().NotBeNull();
                actual.ShipSatellite.Should().NotBeNull();
                actual.ShipSatellite.UpdateDateTime.Should().BeOnOrAfter(startTime)
                    .And.BeOnOrBefore(endTime);
            }
        }
    }
}
