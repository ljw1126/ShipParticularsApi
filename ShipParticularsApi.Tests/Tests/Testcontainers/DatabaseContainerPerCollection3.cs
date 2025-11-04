using FluentAssertions;
using ShipParticularsApi.Repositories;
using Xunit.Abstractions;

namespace ShipParticularsApi.Tests.Tests.Testcontainers
{
    [Collection("Database Collection")]
    public class DatabaseContainerPerCollection3(DatabaseFixture fixture, ITestOutputHelper output)
        : IDisposable
    {
        public void Dispose()
        {
            output.WriteLine($"Collection Container Id = {fixture.ContainerId}");
        }

        [Fact(DisplayName = "shipKey에 해당하는 선박 정보가 없으면 false를 반환한다")]
        public async Task None_ExistsByShipKeyAsync()
        {
            await using var context = fixture.CreateContext();
            await using var transaction = await context.Database.BeginTransactionAsync();

            var repository = new ShipInfoRepository(context);

            bool actual = await repository.ExistsByShipKeyAsync("SHIP_KEY");

            actual.Should().BeFalse();
        }
    }
}
