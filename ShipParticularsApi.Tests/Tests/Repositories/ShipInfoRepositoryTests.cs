using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShipParticularsApi.Contexts;
using ShipParticularsApi.Repositories;
using Xunit.Abstractions;

using static ShipParticularsApi.Tests.Tests.Builders.Entities.ShipInfoTestBuilder;


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
            const string shipKey = "UNIQUE_SHIP_KEY";

            await using (var arrangeContext = CreateContext())
            {
                arrangeContext.ShipInfos.Add(NoService(shipKey, 1L).Build());
                await arrangeContext.SaveChangesAsync();
            }

            // Act & Assert
            await using (var dbContext = CreateContext())
            {
                var repository = new ShipInfoRepository(dbContext);

                var actual = await repository.GetByShipKeyAsync(shipKey);

                actual.Should().NotBeNull();

                var entry = dbContext.Entry(actual!);
                entry.State.Should().Be(EntityState.Unchanged);
            }
        }

        [Fact(DisplayName = "AsNoTracking()으로 조회한 엔티티는 상태추적하지 않는다.")]
        public async Task AsNoTracking()
        {
            // Arrange
            const string shipKey = "UNIQUE_SHIP_KEY";

            await using (var arrangeContext = CreateContext())
            {
                arrangeContext.ShipInfos.Add(NoService(shipKey, 1L).Build());
                await arrangeContext.SaveChangesAsync();
            }

            // Act & Assert
            await using (var dbContext = CreateContext())
            {
                var repository = new ShipInfoRepository(dbContext);

                var actual = await repository.GetReadOnlyByShipKeyAsync(shipKey);

                actual.Should().NotBeNull();

                var entry = dbContext.Entry(actual!);
                entry.State.Should().Be(EntityState.Detached);
            }
        }

        [Fact(DisplayName = "shipKey에 해당하는 선박 정보가 있으면 true를 반환한다")]
        public async Task ExistsByShipKeyAsync()
        {
            // Arrange
            const string shipKey = "UNIQUE_SHIP_KEY";

            await using (var arrangeContext = CreateContext())
            {
                arrangeContext.ShipInfos.Add(NoService(shipKey, 1L).Build());
                await arrangeContext.SaveChangesAsync();
            }

            // Act & Assert
            await using (var dbContext = CreateContext())
            {
                var repository = new ShipInfoRepository(dbContext);

                bool actual = await repository.ExistsByShipKeyAsync(shipKey);

                actual.Should().BeTrue();
            }
        }

        [Fact(DisplayName = "shipKey에 해당하는 선박 정보가 없으면 false를 반환한다")]
        public async Task None_ExistsByShipKeyAsync()
        {
            await using var dbContext = CreateContext();
            var repository = new ShipInfoRepository(dbContext);

            bool actual = await repository.ExistsByShipKeyAsync("UNIQUE_SHIP_KEY");

            actual.Should().BeFalse();
        }
    }
}
