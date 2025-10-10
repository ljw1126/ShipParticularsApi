using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ShipParticularsApi.Contexts;
using ShipParticularsApi.Entities;
using Xunit;
using Xunit.Abstractions;

using static ShipParticularsApi.Tests.Builders.ShipInfoTestBuilder;

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

            ShipInfo newShip = ShipInfo()
                .WithShipKey("CREATE01")
                .Build();

            context.ShipInfos.Add(newShip);
            await context.SaveChangesAsync();

            // Act, Assert
            newShip.Id.Should().Be(1L);
        }

        [Fact]
        public async Task A_tracked_entity_is_retrieved_from_cache_when_quried_by_pk()
        {
            // Arrange
            await using var context = CreateContext();

            ShipInfo newShip = ShipInfo()
                .WithShipKey("CREATE01")
                .WithShipType(ShipTypes.Fishing)
                .Build();

            context.ShipInfos.Add(newShip);
            await context.SaveChangesAsync();

            // Act
            var savedShip = await context.ShipInfos.SingleAsync(s => s.Id == newShip.Id);

            // Assert
            savedShip.Should().BeEquivalentTo(newShip);
            savedShip.Id.Should().Be(1L);
            savedShip.ShipType.Should().Be(ShipTypes.Fishing);
            savedShip.IsService.Should().BeTrue();
        }

        [Fact]
        public async Task A_new_instance_is_retrieved_from_db_when_queried_by_AsNoTracking()
        {
            // Arrange
            await using var context = CreateContext();

            ShipInfo newShip = ShipInfo()
                .WithShipKey("CREATE01")
                .Build();

            context.ShipInfos.Add(newShip);
            await context.SaveChangesAsync();

            // Act
            var savedShip = await context.ShipInfos
                    .AsNoTracking()
                    .SingleAsync(s => s.Id == newShip.Id);

            // Assert
            savedShip.Should().NotBe(newShip);
        }

        [Fact]
        public async Task A_new_test_run_starts_with_an_empty_table()
        {
            // Arrange
            await using var context = CreateContext();

            // Act
            var actual = await context.ShipInfos.LongCountAsync();

            // Assert
            actual.Should().Be(0L);
        }

        [Fact]
        public async Task Read_shipinfo_only_active_service()
        {
            //Arrange
            await using var context = CreateContext();
            context.ShipInfos.Add(ShipInfo()
                .WithShipKey("CREATE01")
                .WithShipName("ALPHA")
                .WithIsService(true)
                .Build());

            context.ShipInfos.Add(ShipInfo()
                .WithShipKey("CREATE02")
                .WithShipName("BETA")
                .WithIsService(false)
                .Build());

            await context.SaveChangesAsync();

            // Act, Assert
            var actual = await context.ShipInfos.Where(s => s.IsService == true)
                                                .FirstOrDefaultAsync();

            actual.ShipName.Should().Be("ALPHA");
            actual.IsService.Should().BeTrue();
        }

        [Fact]
        public async Task Update_shipinfo()
        {
            // Arrange
            await using var context = CreateContext();
            context.ShipInfos.Add(ShipInfo().WithShipKey("UPDATE01").WithShipName("Old Name").Build());
            await context.SaveChangesAsync();

            // Act
            var target = await context.ShipInfos.SingleAsync(s => s.ShipKey == "UPDATE01");
            target.ShipName = "New Name";
            target.IsUseAis = true;
            await context.SaveChangesAsync();

            // Assert
            var actual = await context.ShipInfos.AsNoTracking()
                                .SingleAsync(s => s.ShipKey == "UPDATE01");

            actual.ShipName.Should().Be("New Name");
            actual.IsUseAis.Should().BeTrue();
        }

        [Fact]
        public async Task Delete_shipinfo()
        {
            // Arrange
            await using var context = CreateContext();

            context.ShipInfos.Add(ShipInfo().WithShipKey("DELETE01").Build());
            await context.SaveChangesAsync();

            // Act
            var target = await context.ShipInfos.SingleAsync(s => s.ShipKey == "DELETE01");
            context.ShipInfos.Remove(target);
            await context.SaveChangesAsync();

            // Assert
            var actual = await context.ShipInfos.FirstOrDefaultAsync(s => s.ShipKey == "DELETE01");

            actual.Should().BeNull();
        }

    }
}
