using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ShipParticularsApi.Contexts;
using ShipParticularsApi.Repositories;
using Testcontainers.MsSql;
using Xunit.Abstractions;

using static ShipParticularsApi.Tests.Tests.Builders.Entities.ShipInfoTestBuilder;

namespace ShipParticularsApi.Tests.Tests.Testcontainers
{
    public class DatabaseContainerPerTestcs(ITestOutputHelper output)
        : IAsyncLifetime
    {
        private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("qwer1234!@#$")
            .WithCleanUp(true)
            .Build();

        private DbContextOptions<ShipParticularsContext> _options;

        private ShipParticularsContext CreateContext() => new(_options);

        public async Task DisposeAsync()
        {
            await _dbContainer.StopAsync();
        }

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();

            _options = new DbContextOptionsBuilder<ShipParticularsContext>()
                           .UseSqlServer(_dbContainer.GetConnectionString())
                           .UseLazyLoadingProxies()
                           .Options;

            var context = new ShipParticularsContext(_options);
            context.Database.Migrate();
        }


        [Fact(DisplayName = "DB에서 조회한 엔티티는 DbContext가 상태 추적 한다.")]
        public async Task AsTracking()
        {
            // Arrange
            await using var context = CreateContext();
            await using var transaction = await context.Database.BeginTransactionAsync();

            const string shipKey = "SHIP_KEY";

            context.ShipInfos.Add(NoService(shipKey).Build());
            await context.SaveChangesAsync();

            // Act & Assert
            var repository = new ShipInfoRepository(context);
            var actual = await repository.GetByShipKeyAsync(shipKey);

            actual.Should().NotBeNull();

            var entry = context.Entry(actual!);
            entry.State.Should().Be(EntityState.Unchanged);

            // 🌟 await using 블록 종료 시, transaction이 롤백되어 데이터 자동 정리.
        }

        [Fact(DisplayName = "AsNoTracking()으로 조회한 엔티티는 상태추적하지 않는다.")]
        public async Task AsNoTracking()
        {
            // Arrange
            await using var context = CreateContext();
            await using var transaction = await context.Database.BeginTransactionAsync();

            const string shipKey = "SHIP_KEY";

            context.ShipInfos.Add(NoService(shipKey).Build());
            await context.SaveChangesAsync();

            // Act & Assert
            var repository = new ShipInfoRepository(context);
            var actual = await repository.GetReadOnlyByShipKeyAsync(shipKey);

            actual.Should().NotBeNull();

            var entry = context.Entry(actual!);
            entry.State.Should().Be(EntityState.Detached);
        }

        [Fact(DisplayName = "shipKey에 해당하는 선박 정보가 있으면 true를 반환한다")]
        public async Task ExistsByShipKeyAsync()
        {
            // Arrange
            await using var context = CreateContext();
            await using var transaction = await context.Database.BeginTransactionAsync();

            const string shipKey = "SHIP_KEY";

            context.ShipInfos.Add(NoService(shipKey).Build());
            await context.SaveChangesAsync();

            // Act & Assert
            var repository = new ShipInfoRepository(context);
            bool actual = await repository.ExistsByShipKeyAsync(shipKey);

            actual.Should().BeTrue();
        }

        [Fact(DisplayName = "shipKey에 해당하는 선박 정보가 없으면 false를 반환한다")]
        public async Task None_ExistsByShipKeyAsync()
        {
            await using var context = CreateContext();
            await using var transaction = await context.Database.BeginTransactionAsync();

            var repository = new ShipInfoRepository(context);

            bool actual = await repository.ExistsByShipKeyAsync("SHIP_KEY");

            actual.Should().BeFalse();
        }
    }
}
