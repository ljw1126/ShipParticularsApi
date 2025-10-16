using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ShipParticularsApi.Contexts;
using ShipParticularsApi.Entities;
using Xunit;
using Xunit.Abstractions;
using static ShipParticularsApi.Tests.Builders.ReplaceShipNameTestBuilder;
using static ShipParticularsApi.Tests.Builders.ShipInfoTestBuilder;
using static ShipParticularsApi.Tests.Builders.ShipSatelliteTestBuilder;
using static ShipParticularsApi.Tests.Builders.ShipServiceTestBuilder;
using static ShipParticularsApi.Tests.Builders.SkTelinkCompanyShipTestBuilder;

namespace ShipParticularsApi.Tests.Examples
{
    public class NavigationPropertyTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<ShipParticularsContext> _options;
        private readonly ITestOutputHelper _output;

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
            // Arrange
            await using (var arrangeContext = CreateContext())
            {
                arrangeContext.ShipInfos.Add(ShipInfo()
                    .WithShipKey("SHIP01")
                    .WithReplaceShipName(ReplaceShipName().WithReplaceShipName("Next Vessel"))
                    .WithShipServices(
                        ShipService().WithServiceName(ServiceNameTypes.Cctv).WithIsCompleted(true),
                        ShipService().WithServiceName(ServiceNameTypes.EuMrv).WithIsCompleted(true),
                        ShipService().WithServiceName(ServiceNameTypes.NoonReport).WithIsCompleted(false)
                    )
                    .Build());
                await arrangeContext.SaveChangesAsync();
            }

            // Act, Assert
            await using (var assertContext = CreateContext())
            {
                var savedShipInfo = await assertContext.ShipInfos
                    .Include(s => s.ReplaceShipName)
                    .Include(s => s.ShipServices)
                    .AsSplitQuery()
                    .SingleAsync(s => s.ShipKey == "SHIP01");

                var shipServices = savedShipInfo.ShipServices;
                var replaceShipName = savedShipInfo.ReplaceShipName;

                shipServices.Should().NotBeNullOrEmpty()
                        .And.HaveCount(3);
                replaceShipName.Should().NotBeNull();
            }
        }

        [Fact(DisplayName = "AIS 서비스 사용중 AIS 비활성화하면, 서비스 컬렉션에서 SatAis만 제거된다.")]
        public async Task Ais_toggle_off_test()
        {
            await using (var arrangeContext = CreateContext())
            {
                arrangeContext.ShipInfos.Add(ShipInfo()
                    .WithShipKey("TEST_SHIP_KEY")
                    .WithShipServices(
                        ShipService().WithServiceName(ServiceNameTypes.SatAis).WithIsCompleted(true),
                        ShipService().WithServiceName(ServiceNameTypes.KtSat).WithIsCompleted(true)
                    )
                    .Build());
                await arrangeContext.SaveChangesAsync();
            }

            await using (var actContext = CreateContext())
            {
                ShipInfo target = await actContext.ShipInfos
                    .Include(s => s.ShipServices)
                    .SingleAsync(s => s.ShipKey == "TEST_SHIP_KEY");

                target.ManageAisService(false);

                await actContext.SaveChangesAsync();
            }

            await using (var assertContext = CreateContext())
            {
                ShipInfo target = await assertContext.ShipInfos
                    .Include(s => s.ShipServices)
                    .SingleAsync(s => s.ShipKey == "TEST_SHIP_KEY");

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
            await using (var arrangeContext = CreateContext())
            {
                arrangeContext.ShipInfos.Add(ShipInfo()
                      .WithShipKey("UNIQUE_SHIP_KEY")
                      .WithShipServices(KtSatService("UNIQUE_SHIP_KEY"))
                      .WithShipSatellite(SkTelinkSatellite("UNIQUE_SHIP_KEY", "SATELLITE_ID"))
                      .WithExternalShipId("SATELLITE_ID")
                      .WithIsUseKtsat(true)
                      .WithSkTelinkCompanyShip(SkTelinkCompanyShip("UNIQUE_SHIP_KEY", "UNIQUE_COMPANY_NAME"))
                      .Build());
                await arrangeContext.SaveChangesAsync();
            }

            await using (var actContext = CreateContext())
            {
                ShipInfo target = await actContext.ShipInfos
                    .Include(s => s.ShipServices)
                    .Include(s => s.ShipSatellite)
                    .Include(s => s.SkTelinkCompanyShip)
                    .AsSplitQuery()
                    .SingleAsync(s => s.ShipKey == "UNIQUE_SHIP_KEY");

                target.ManageGpsService(false, null, null, null);

                await actContext.SaveChangesAsync();
            }

            await using (var assertContext = CreateContext())
            {
                ShipInfo actual = await assertContext.ShipInfos
                    .Include(s => s.ShipServices)
                    .Include(s => s.ShipSatellite)
                    .Include(s => s.SkTelinkCompanyShip)
                    .AsSplitQuery()
                    .SingleAsync(s => s.ShipKey == "UNIQUE_SHIP_KEY");

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
