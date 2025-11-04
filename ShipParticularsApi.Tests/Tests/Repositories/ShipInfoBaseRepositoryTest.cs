using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ShipParticularsApi.Repositories;
using ShipParticularsApi.Tests.Tests.Testcontainers;
using Xunit.Abstractions;

using static ShipParticularsApi.Tests.Tests.Builders.Entities.ShipInfoTestBuilder;

namespace ShipParticularsApi.Tests.Tests.Repositories
{
    public class ShipInfoBaseRepositoryTest(DatabaseFixture fixture, ITestOutputHelper output)
        : BaseRepositoryTest(fixture, output), IClassFixture<DatabaseFixture>
    {
        [Fact(DisplayName = "DB에서 조회한 엔티티는 DbContext가 상태 추적 한다.")]
        public async Task AsTracking()
        {
            // Arrange
            const string shipKey = "SHIP_KEY";
            Context.ShipInfos.Add(NoService(shipKey).Build());
            await Context.SaveChangesAsync();

            // Act & Assert
            var repository = new ShipInfoRepository(Context);
            var actual = await repository.GetByShipKeyAsync(shipKey);

            actual.Should().NotBeNull();

            var entry = Context.Entry(actual!);
            entry.State.Should().Be(EntityState.Unchanged);
        }

        [Fact(DisplayName = "AsNoTracking()으로 조회한 엔티티는 상태추적하지 않는다.")]
        public async Task AsNoTracking()
        {
            // Arrange
            const string shipKey = "SHIP_KEY";

            Context.ShipInfos.Add(NoService(shipKey).Build());
            await Context.SaveChangesAsync();

            // Act & Assert
            var repository = new ShipInfoRepository(Context);
            var actual = await repository.GetReadOnlyByShipKeyAsync(shipKey);

            actual.Should().NotBeNull();

            var entry = Context.Entry(actual!);
            entry.State.Should().Be(EntityState.Detached);
        }

        [Fact(DisplayName = "shipKey에 해당하는 선박 정보가 있으면 true를 반환한다")]
        public async Task ExistsByShipKeyAsync()
        {
            // Arrange
            const string shipKey = "SHIP_KEY";
            Context.ShipInfos.Add(NoService(shipKey).Build());
            await Context.SaveChangesAsync();

            // Act & Assert
            var repository = new ShipInfoRepository(Context);
            bool actual = await repository.ExistsByShipKeyAsync(shipKey);
            actual.Should().BeTrue();
        }

        [Fact(DisplayName = "shipKey에 해당하는 선박 정보가 없으면 false를 반환한다")]
        public async Task None_ExistsByShipKeyAsync()
        {
            var repository = new ShipInfoRepository(Context);

            bool actual = await repository.ExistsByShipKeyAsync("SHIP_KEY");

            actual.Should().BeFalse();
        }


    }
}
