using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ShipParticularsApi.Repositories;
using Xunit.Abstractions;
using static ShipParticularsApi.Tests.Tests.Builders.Entities.ShipInfoTestBuilder;

namespace ShipParticularsApi.Tests.Tests.Testcontainers
{
    [CollectionDefinition("Database Collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>;

    [Collection("Database Collection")]
    public class DatabaseContainerPerCollection(DatabaseFixture fixture, ITestOutputHelper output)
        : IDisposable
    {
        public void Dispose()
        {
            output.WriteLine($"Collection Container Id = {fixture.ContainerId}");
        }

        [Fact(DisplayName = "DB에서 조회한 엔티티는 DbContext가 상태 추적 한다.")]
        public async Task AsTracking()
        {
            // Arrange
            await using var context = fixture.CreateContext();
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
            await using var context = fixture.CreateContext();
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
    }
}
