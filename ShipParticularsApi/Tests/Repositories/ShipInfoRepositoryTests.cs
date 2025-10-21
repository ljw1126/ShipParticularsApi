using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ShipParticularsApi.Contexts;
using ShipParticularsApi.Entities.Enums;
using ShipParticularsApi.Repositories;
using Xunit;
using Xunit.Abstractions;

using static ShipParticularsApi.Tests.Builders.ShipInfoTestBuilder;
using static ShipParticularsApi.Tests.Builders.ShipSatelliteTestBuilder;
using static ShipParticularsApi.Tests.Builders.ShipServiceTestBuilder;


namespace ShipParticularsApi.Tests.Repositories
{
    public class ShipInfoRepositoryTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<ShipParticularsContext> _options;
        private readonly ITestOutputHelper _output;

        // NOTE: beforeEach
        public ShipInfoRepositoryTests(ITestOutputHelper output)
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

        [Fact(DisplayName = "DB에서 조회한 엔티티는 DbContext가 상태 추적 한다.")]
        public async Task AsTracking()
        {
            // Arrange
            const string shipKey = "SHIP_KEY";
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

            await using (var arrangeContext = CreateContext())
            {
                arrangeContext.ShipInfos.Add(newShipInfo);
                await arrangeContext.SaveChangesAsync();
            }

            var dbContext = CreateContext();
            var repository = new ShipInfoRepository(dbContext);

            // Act
            var savedShipInfo = await repository.GetByShipKeyAsync(shipKey);

            // Assert
            savedShipInfo.Should().NotBeNull();
            var entry = dbContext.Entry(savedShipInfo!);
            entry.State.Should().Be(EntityState.Unchanged);
        }

        [Fact(DisplayName = "AsNoTracking()으로 조회한 엔티티는 상태추적하지 않는다.")]
        public async Task AsNoTracking()
        {
            // Arrange
            const string shipKey = "SHIP_KEY";
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

            await using (var arrangeContext = CreateContext())
            {
                arrangeContext.ShipInfos.Add(newShipInfo);
                await arrangeContext.SaveChangesAsync();
            }

            var dbContext = CreateContext();
            var repository = new ShipInfoRepository(dbContext);

            // Act
            var savedShipInfo = await repository.GetReadOnlyByShipKeyAsync(shipKey);

            // Assert
            savedShipInfo.Should().NotBeNull();
            var entry = dbContext.Entry(savedShipInfo!);
            entry.State.Should().Be(EntityState.Detached);
        }
    }
}
