using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ShipParticularsApi.Contexts;
using ShipParticularsApi.Entities.Enums;
using ShipParticularsApi.ValueObjects;
using Xunit;
using Xunit.Abstractions;
using static ShipParticularsApi.Tests.Builders.ShipInfoTestBuilder;
using static ShipParticularsApi.Tests.Builders.ShipSatelliteTestBuilder;
using static ShipParticularsApi.Tests.Builders.ShipServiceTestBuilder;

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
            var newShipInfo = ShipInfo()
                   .WithShipKey(shipKey)
                   .WithCallsign("TEST_CALLSIGN")
                   .WithShipName("TEST_SHIP_NAMME")
                   .WithShipType(ShipTypes.Fishing)
                   .WithShipCode("TEST_SHIP_CODE")
                   .WithShipServices(KtSatService(shipKey))
                   .WithShipSatellite(KtSatellite(shipKey, "SATELLITE_ID"))
                   .WithExternalShipId("SATELLITE_ID")
                   .WithIsUseKtsat(true)
                   .Build();

            var startTime = DateTime.UtcNow;

            await using (var arrangeContext = CreateContext())
            {
                arrangeContext.ShipInfos.Add(newShipInfo);
                await arrangeContext.SaveChangesAsync();
            }

            var endTime = DateTime.UtcNow;

            // Act, Assert
            await using (var assertContext = CreateContext())
            {
                var actual = await assertContext.ShipInfos
                    .Include(s => s.ShipSatellite)
                    .SingleOrDefaultAsync(s => s.ShipKey == shipKey);

                actual.Should().NotBeNull();
                actual.ShipSatellite.Should().NotBeNull();
                actual.ShipSatellite.CreateDateTime.Should().BeOnOrAfter(startTime)
                    .And.BeOnOrBefore(endTime);
                actual.ShipSatellite.UpdateDateTime.Should().Be(default);
            }
        }

        [Fact]
        public async Task UpdateDateTime_test()
        {
            // Arrange
            var startTime = DateTime.UtcNow;
            const string shipKey = "UNIQUE_SHIP_KEY";
            var newShipInfo = ShipInfo()
                   .WithShipKey(shipKey)
                   .WithCallsign("TEST_CALLSIGN")
                   .WithShipName("TEST_SHIP_NAMME")
                   .WithShipType(ShipTypes.Fishing)
                   .WithShipCode("TEST_SHIP_CODE")
                   .WithShipServices(KtSatService(shipKey))
                   .WithShipSatellite(
                        ShipSatellite()
                        .WithShipKey("UNIQUE_SHIP_KEY")
                        .WithSatelliteType(SatelliteTypes.KtSat)
                        .WithSatelliteId("SATELLITE_ID")
                        .WithIsUseSatellite(true)
                        .WithCreateUserId(FixedUserId)
                        .Build()
                   )
                   .WithExternalShipId("SATELLITE_ID")
                   .WithIsUseKtsat(true)
                   .Build();

            await using (var arrangeContext = CreateContext())
            {
                arrangeContext.ShipInfos.Add(newShipInfo);
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
                    .SingleOrDefaultAsync(s => s.ShipKey == shipKey);

                shipInfo.ManageGpsService(true, new SatelliteDetails("SATELLITE_ID", "SK_TELINK", "COMPANY_NAME"), FixedUserId);
                await actConext.SaveChangesAsync();
            }

            var endTime = DateTime.UtcNow;

            // Assert
            await using (var assertContext = CreateContext())
            {
                var actual = await assertContext.ShipInfos
                    .Include(s => s.ShipSatellite)
                    .SingleOrDefaultAsync(s => s.ShipKey == shipKey);

                actual.Should().NotBeNull();
                actual.ShipSatellite.Should().NotBeNull();
                actual.ShipSatellite.UpdateDateTime.Should().BeOnOrAfter(startTime)
                    .And.BeOnOrBefore(endTime);
            }
        }
    }
}
