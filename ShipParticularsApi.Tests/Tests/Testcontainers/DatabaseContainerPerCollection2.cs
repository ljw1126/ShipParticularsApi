using FluentAssertions;
using ShipParticularsApi.Repositories;
using Xunit.Abstractions;
using static ShipParticularsApi.Tests.Tests.Builders.Entities.ShipInfoTestBuilder;

namespace ShipParticularsApi.Tests.Tests.Testcontainers
{
    [Collection("Database Collection")]
    public class DatabaseContainerPerCollection2(DatabaseFixture fixture, ITestOutputHelper output)
        : IDisposable
    {
        public void Dispose()
        {
            output.WriteLine($"Collection Container Id = {fixture.ContainerId}");
        }

        [Fact(DisplayName = "shipKey에 해당하는 선박 정보가 있으면 true를 반환한다")]
        public async Task ExistsByShipKeyAsync()
        {
            // Arrange
            await using var context = fixture.CreateContext();
            await using var transaction = await context.Database.BeginTransactionAsync();

            const string shipKey = "SHIP_KEY";

            context.ShipInfos.Add(NoService(shipKey).Build());
            await context.SaveChangesAsync();

            // Act & Assert
            var repository = new ShipInfoRepository(context);
            bool actual = await repository.ExistsByShipKeyAsync(shipKey);

            actual.Should().BeTrue();
        }
    }
}
