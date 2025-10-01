using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ShipParticularsApi.Contexts;
using ShipParticularsApi.Entities;
using Xunit;
using Xunit.Abstractions;

namespace ShipParticularsApi.Tests
{
    public class BasicPersistenceCrudTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<ShipParticularsContext> _options;
        private readonly ITestOutputHelper _output;

        // NOTE: beforeEach
        public BasicPersistenceCrudTests(ITestOutputHelper output)
        {
            _output = output;

            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            _options = new DbContextOptionsBuilder<ShipParticularsContext>()
                .UseSqlite(_connection)
                .Options;

            var context = new ShipParticularsContext(_options);
            context.Database.EnsureCreated();
        }

        // NOTE: AfterEach
        public void Dispose() => _connection.Dispose();

        ShipParticularsContext CreateContext() => new(_options);

        [Fact]
        public async Task Create_new_shipinfo()
        {
            // Arrange
            await using var context = CreateContext();

            var newShip = new ShipInfo
            {
                ShipKey = "CREATE01",
                ShipName = "New Vessel",
                Callsign = "CALL01",
                ShipType = "-"
            };

            context.ShipInfos.Add(newShip);
            await context.SaveChangesAsync();

            // Act, Assert
            Assert.Equal(1L, newShip.Id);
        }

        [Fact]
        public async Task A_tracked_entity_is_retrieved_from_cache_when_quried_by_pk()
        {
            // Arrange
            await using var context = CreateContext();

            var newShip = new ShipInfo
            {
                ShipKey = "CREATE01",
                ShipName = "New Vessel",
                Callsign = "CALL01"
            };

            context.ShipInfos.Add(newShip);
            await context.SaveChangesAsync();

            // Act
            var savedShip = await context.ShipInfos.SingleAsync(s => s.Id == newShip.Id);

            // Assert
            Assert.Same(newShip, savedShip);
            Assert.Equal(1, savedShip.Id);
            Assert.Equal("-", savedShip.ShipType);
            Assert.True(savedShip.IsService);
        }

        [Fact]
        public async Task A_new_instance_is_retrieved_from_db_when_queried_by_AsNoTracking()
        {
            // Arrange
            await using var context = CreateContext();

            var newShip = new ShipInfo
            {
                ShipKey = "CREATE01",
                ShipName = "New Vessel",
                Callsign = "CALL01",
                ShipType = "-"
            };

            context.ShipInfos.Add(newShip);
            await context.SaveChangesAsync();

            // Act
            var savedShip = await context.ShipInfos
                    .AsNoTracking()
                    .SingleAsync(s => s.Id == newShip.Id);

            // Assert
            Assert.NotSame(newShip, savedShip);
        }

        [Fact]
        public async Task A_new_test_run_starts_with_an_empty_table()
        {
            // Arrange
            await using var context = CreateContext();

            // Act
            var actual = await context.ShipInfos.LongCountAsync();

            // Assert
            Assert.Equal(0L, actual);
        }

        [Fact]
        public async Task Read_shipinfo_only_active_service()
        {
            //Arrange
            await using var context = CreateContext();
            context.ShipInfos.Add(new ShipInfo
            {
                ShipKey = "CREATE01",
                ShipName = "ALPHA",
                Callsign = "CALL01",
                IsService = true
            });

            context.ShipInfos.Add(new ShipInfo
            {
                ShipKey = "CREATE02",
                ShipName = "BETA",
                Callsign = "CALL02",
                IsService = false
            });

            await context.SaveChangesAsync();

            // Act
            var shipInfo = await context.ShipInfos.Where(s => s.IsService == true)
                                                .FirstOrDefaultAsync();

            // Assert
            Assert.Equal("ALPHA", shipInfo.ShipName);
            Assert.True(shipInfo.IsService);
        }

        [Fact]
        public async Task Update_shipinfo()
        {
            // Arrange
            await using var context = CreateContext();
            var originalShip = new ShipInfo { ShipKey = "UPDATE01", ShipName = "Old Name", Callsign = "CALL01" };
            context.ShipInfos.Add(originalShip);
            await context.SaveChangesAsync();

            // Act
            var target = await context.ShipInfos.SingleAsync(s => s.ShipKey == "UPDATE01");
            target.ShipName = "New Name";
            target.IsUseAis = true;
            await context.SaveChangesAsync();

            // Assert
            var actual = await context.ShipInfos.AsNoTracking()
                                .SingleAsync(s => s.ShipKey == "UPDATE01");

            Assert.Equal("New Name", actual.ShipName);
            Assert.True(actual.IsUseAis);
        }

        [Fact]
        public async Task Delete_shipinfo()
        {
            // Arrange
            await using var context = CreateContext();
            var originalShip = new ShipInfo { ShipKey = "DELETE01", ShipName = "To Delete", Callsign = "CALL01" };
            context.ShipInfos.Add(originalShip);
            await context.SaveChangesAsync();

            // Act
            var target = await context.ShipInfos.SingleAsync(s => s.ShipKey == "DELETE01");
            context.ShipInfos.Remove(target);
            await context.SaveChangesAsync();

            // Assert
            var actual = await context.ShipInfos.FirstOrDefaultAsync(s => s.ShipKey == "DELETE01");

            Assert.Null(actual);
        }

    }
}
