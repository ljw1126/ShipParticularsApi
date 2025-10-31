using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShipParticularsApi.Contexts;
using ShipParticularsApi.Entities;
using Xunit.Abstractions;
using static ShipParticularsApi.Tests.Tests.Builders.Entities.ReplaceShipNameTestBuilder;
using static ShipParticularsApi.Tests.Tests.Builders.Entities.ShipInfoTestBuilder;
using static ShipParticularsApi.Tests.Tests.Builders.Entities.ShipServiceTestBuilder;

namespace ShipParticularsApi.Tests.Tests.Examples
{
    public class NavigationPropertyTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<ShipParticularsContext> _options;
        private readonly ITestOutputHelper _output;
        private const string FixedUserId = "TEST_USER_01";

        // NOTE: beforeEach
        public NavigationPropertyTests(ITestOutputHelper output)
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
            context.Database.EnsureCreated();
        }

        // NOTE: AfterEach
        public void Dispose() => _connection.Dispose();

        ShipParticularsContext CreateContext() => new(_options);

        [Fact]
        public async Task SplitQueryTest()
        {
            const string shipKey = "SHIP01";

            // Arrange
            await using (var arrangeContext = CreateContext())
            {
                arrangeContext.ShipInfos.Add(NoService(shipKey)
                        .WithReplaceShipName(ReplaceShipName().WithShipKey(shipKey).WithReplaceShipName("Next Vessel"))
                        .WithShipServices(
                            ShipService().WithShipKey(shipKey).WithServiceName(ServiceNameTypes.Cctv).WithIsCompleted(true),
                            ShipService().WithShipKey(shipKey).WithServiceName(ServiceNameTypes.EuMrv).WithIsCompleted(true),
                            ShipService().WithShipKey(shipKey).WithServiceName(ServiceNameTypes.NoonReport).WithIsCompleted(false)
                        )
                        .Build());
                await arrangeContext.SaveChangesAsync();
            }

            // Act, Assert
            await using (var assertContext = CreateContext())
            {
                var savedShipInfo = await assertContext.ShipInfos
                    .AsNoTracking()
                    .Include(s => s.ReplaceShipName)
                    .Include(s => s.ShipServices)
                    .AsSplitQuery()
                    .SingleAsync(s => s.ShipKey == shipKey && s.IsService == true);

                savedShipInfo.ShipServices.Should().NotBeNullOrEmpty()
                        .And.HaveCount(3);
                savedShipInfo.ReplaceShipName.Should().NotBeNull();
            }
        }

        [Fact(DisplayName = "AIS 서비스 사용중 AIS 비활성화하면, 서비스 컬렉션에서 SatAis만 제거된다.")]
        public async Task Ais_toggle_off_test()
        {
            const string shipKey = "UNIQUE_SHIP_KEY";

            await using (var arrangeContext = CreateContext())
            {
                arrangeContext.ShipInfos.Add(
                    NoService(shipKey).WithShipServices(
                        SatAisService(shipKey),
                        KtSatService(shipKey)
                    ).Build()
                );
                await arrangeContext.SaveChangesAsync();
            }

            await using (var actContext = CreateContext())
            {
                ShipInfo target = await actContext.ShipInfos
                    .Include(s => s.ShipServices)
                    .SingleAsync(s => s.ShipKey == shipKey && s.IsService == true);

                target.DeactiveAisService();

                await actContext.SaveChangesAsync();
            }

            await using (var assertContext = CreateContext())
            {
                ShipInfo target = await assertContext.ShipInfos
                    .AsNoTracking()
                    .Include(s => s.ShipServices)
                    .SingleAsync(s => s.ShipKey == shipKey && s.IsService == true);

                target.Should().NotBeNull();
                target.ShipServices.Should().NotBeEmpty()
                    .And.HaveCount(1);
                target.ShipServices.Should().ContainSingle()
                    .Which.ServiceName.Should().Be(ServiceNameTypes.KtSat);
            }
        }

        [Fact(DisplayName = "SK TELINK 위성 사용 중에 GPS 비활성화하면, 모든 관련 자식 엔티티가 DELETE 되고, 부모 컬럼도 업데이트 된다")]
        public async Task Gps_toggle_off_test()
        {
            const string shipKey = "UNIQUE_SHIP_KEY";

            await using (var arrangeContext = CreateContext())
            {
                arrangeContext.ShipInfos.Add(UsingSkTelink(shipKey, FixedUserId, 0L).Build());
                await arrangeContext.SaveChangesAsync();
            }

            await using (var actContext = CreateContext())
            {
                ShipInfo target = await actContext.ShipInfos
                    .Include(s => s.ShipServices)
                    .Include(s => s.ShipSatellite)
                    .Include(s => s.SkTelinkCompanyShip)
                    .AsSplitQuery()
                    .SingleAsync(s => s.ShipKey == shipKey && s.IsService == true);

                target.DeactiveGpsService();

                await actContext.SaveChangesAsync();
            }

            await using (var assertContext = CreateContext())
            {
                ShipInfo actual = await assertContext.ShipInfos
                    .AsNoTracking()
                    .Include(s => s.ShipServices)
                    .Include(s => s.ShipSatellite)
                    .Include(s => s.SkTelinkCompanyShip)
                    .AsSplitQuery()
                    .SingleAsync(s => s.ShipKey == shipKey && s.IsService == true);

                actual.Should().NotBeNull();
                actual.ExternalShipId.Should().BeNull();
                actual.IsUseKtsat.Should().BeFalse();

                actual.ShipServices.Should().BeEmpty();
                actual.ShipSatellite.Should().BeNull();
                actual.SkTelinkCompanyShip.Should().BeNull();
            }
        }
    }
}
