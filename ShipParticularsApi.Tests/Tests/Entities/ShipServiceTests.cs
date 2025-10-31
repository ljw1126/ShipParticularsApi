using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ShipParticularsApi.Contexts;
using ShipParticularsApi.Entities;
using static ShipParticularsApi.Tests.Tests.Builders.Entities.ShipInfoTestBuilder;
using static ShipParticularsApi.Tests.Tests.Builders.Entities.ShipServiceTestBuilder;

namespace ShipParticularsApi.Tests.Tests.Entities
{
    public class ShipServiceTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<ShipParticularsContext> _options;

        // NOTE: beforeEach
        public ShipServiceTests()
        {
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

        // NOTE. Text Fixture 재활용해 검증시 실패
        [Fact]
        public async Task Save_shipService()
        {
            // Arrange
            const string shipKey = "UNIQUE_SHIP_KEY";
            await using var context = CreateContext();

            ShipInfo newShip = NoService(shipKey).Build();
            newShip.ShipServices.Add(SatAisService(shipKey).Build());
            context.ShipInfos.Add(newShip);

            await context.SaveChangesAsync();

            // Act & Assert
            var actual = await context.ShipServices.AsNoTracking()
                               .SingleAsync(s => s.ShipKey == shipKey && s.IsCompleted == true);

            var expected = ShipService().WithId(1L)
                            .WithIsCompleted(true)
                            .WithServiceName(ServiceNameTypes.SatAis)
                            .WithShipKey(shipKey)
                            .Build();

            actual.Should().BeEquivalentTo(expected);
        }
    }
}
