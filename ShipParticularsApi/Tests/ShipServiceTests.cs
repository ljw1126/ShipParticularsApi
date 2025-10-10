using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ShipParticularsApi.Contexts;
using ShipParticularsApi.Entities;
using Xunit;

using static ShipParticularsApi.Tests.Builders.ShipInfoTestBuilder;
using static ShipParticularsApi.Tests.Builders.ShipServiceTestBuilder;

namespace ShipParticularsApi.Tests
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

        [Fact]
        public async Task Save_shipService()
        {
            // Arrange
            await using var context = CreateContext();

            ShipInfo newShip = ShipInfo()
                .WithShipKey("TEST_KEY")
                .Build();

            ShipService satAisShipService = ShipService()
                .WithServiceName(ServiceNameTypes.SatAis)
                .WithIsCompleted(true)
                .Build();

            newShip.ShipServices.Add(satAisShipService);
            context.ShipInfos.Add(newShip);

            await context.SaveChangesAsync();

            // Act & Assert
            var actual = await context.ShipServices.AsNoTracking()
                               .SingleAsync(s => s.ShipKey == "TEST_KEY");
            var expected = ShipService().WithId(1L)
                            .WithIsCompleted(true)
                            .WithServiceName(ServiceNameTypes.SatAis)
                            .WithShipKey("TEST_KEY");
            Assert.Equivalent(expected, actual);
        }
    }
}
