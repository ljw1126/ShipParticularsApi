using FluentAssertions;
using ShipParticularsApi.Contexts;
using ShipParticularsApi.Entities.Enums;
using ShipParticularsApi.Repositories;
using ShipParticularsApi.Services;
using ShipParticularsApi.Services.Dtos.Params;
using ShipParticularsApi.Tests.Helper;
using Xunit;
using Xunit.Abstractions;
using static ShipParticularsApi.Tests.Builders.Entities.ShipInfoTestBuilder;
using static ShipParticularsApi.Tests.Builders.Entities.ShipSatelliteTestBuilder;
using static ShipParticularsApi.Tests.Builders.Entities.ShipServiceTestBuilder;
using static ShipParticularsApi.Tests.Builders.Entities.SkTelinkCompanyShipTestBuilder;

// TODO. TransactionDecorator (sut) 획득하는 코드 중복
// TODO. 검증 부분 중복
// TODO. 기존 ShipInfo가 있을때 초기화 중복
namespace ShipParticularsApi.Tests.Services
{
    public class ShipParticularsServiceIntegrationTests
        : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly ITestOutputHelper _output;

        public ShipParticularsServiceIntegrationTests(
            CustomWebApplicationFactory<Program> factory,
            ITestOutputHelper output
        )
        {
            _factory = factory;
            _output = output;
            DbInit();
        }

        private void DbInit()
        {
            var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ShipParticularsContext>();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }

        [Fact]
        public void Test()
        {
            // Arrange
            // 팩토리를 사용하여 테스트 스코프를 생성합니다. 
            // AddScoped 서비스들이 이 스코프 내에서 올바르게 생성됩니다.
            using var scope = _factory.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            // Act
            // 1. IShipParticularsService 인터페이스를 요청합니다.
            var resolvedService = serviceProvider.GetRequiredService<IShipParticularsService>();

            // 2. 내부에서 데코레이팅되고 있는 ShipParticularsService 객체도 요청합니다.
            // 이는 데코레이터 패턴에서 두 객체 모두 컨테이너에 등록되어 있어야 함을 보장합니다.
            var innerService = serviceProvider.GetService<ShipParticularsService>();

            // Assert (FluentAssertions 사용 가정)
            // 1. IShipParticularsService가 null이 아닌지 확인합니다.
            resolvedService.Should().NotBeNull("IShipParticularsService는 DI 컨테이너에서 해결되어야 합니다.");

            // 2. IShipParticularsService의 실제 타입이 TransactionDecorator인지 확인합니다. 
            //    이는 데코레이터 패턴이 올바르게 적용되었음을 의미합니다.
            resolvedService.Should().BeOfType<TransactionDecorator>(
                "IShipParticularsService는 TransactionDecorator로 래핑되어야 합니다."
            );

            // 3. 내부 서비스도 null이 아닌지 확인합니다 (데코레이터가 내부 객체를 주입받아 생성되었을 테니).
            innerService.Should().NotBeNull("ShipParticularsService 구현체도 DI 컨테이너에서 해결되어야 합니다.");
        }

        [Fact(DisplayName = "신규 ShipInfo (AIS/GPS Toggle off)")]
        public async Task Case1()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var sut = serviceProvider.GetRequiredService<IShipParticularsService>();

            var testShipKey = "TEST_SHIP_KEY";
            var param = new ShipParticularsParam
            {
                IsAisToggleOn = false,
                IsGPSToggleOn = false,
                ShipKey = testShipKey,
                Callsign = "NEW_CALLSIGN",
                ShipName = "NEW_SHIP_NAME",
                ShipType = "FISHING",
                ShipCode = "NEW_SHIP_CODE"
            };

            // Act
            await sut.Process(param);

            // Assert
            var repository = serviceProvider.GetRequiredService<IShipInfoRepository>();
            var actual = await repository.GetByShipKeyAsync(testShipKey);

            actual.Should().NotBeNull();
            actual.ShipName.Should().Be("NEW_SHIP_NAME");
            actual.ShipType.Should().Be(ShipTypes.Fishing);
        }

        [Fact(DisplayName = "신규 ShipInfo이고, AIS 토글이 On인 경우 ShipServices의 길이는 1이다")]
        public async Task Case2()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var sut = serviceProvider.GetRequiredService<IShipParticularsService>(); // TransactionDecorator

            var param = new ShipParticularsParam
            {
                IsAisToggleOn = true,
                IsGPSToggleOn = false,
                ShipKey = "NEW_SHIP_KEY",
                Callsign = "NEW_CALLSIGN",
                ShipName = "NEW_SHIP_NAME",
                ShipType = "FISHING",
                ShipCode = "NEW_SHIP_CODE"
            };

            // Act
            await sut.Process(param);

            // Assert
            var repository = serviceProvider.GetRequiredService<IShipInfoRepository>();
            var actual = await repository.GetByShipKeyAsync(param.ShipKey);

            actual.Should().NotBeNull();
            actual.IsUseAis.Should().BeTrue();
            actual.ShipServices.Should().ContainSingle();
            actual.ShipServices.Should().ContainEquivalentOf(SatAisService(param.ShipKey), options =>
                options.Including(s => s.ShipKey)
                        .Including(s => s.ServiceName)
            );
        }

        [Fact(DisplayName = "신규 ShipInfo이고, GPS Toggle Off인 경우 ShipServices가 비어있다.")]
        public async Task Case3()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var sut = serviceProvider.GetRequiredService<IShipParticularsService>();

            var param = new ShipParticularsParam
            {
                IsAisToggleOn = false,
                IsGPSToggleOn = false,
                ShipKey = "NEW_SHIP_KEY",
                Callsign = "NEW_CALLSIGN",
                ShipName = "NEW_SHIP_NAME",
                ShipType = "FISHING",
                ShipCode = "NEW_SHIP_CODE"
            };

            // Act
            await sut.Process(param);

            // Assert
            var repository = serviceProvider.GetRequiredService<IShipInfoRepository>();
            var actual = await repository.GetByShipKeyAsync(param.ShipKey);

            actual.Should().NotBeNull();
            actual.IsUseAis.Should().BeFalse();
            actual.ShipServices.Should().BeEmpty();
        }

        [Fact(DisplayName = "신규 ShipInfo이고, KT_SAT 위성을 사용하는 경우 ShipService, ShipSatellite가 추가된다")]
        public async Task Case4()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var sut = serviceProvider.GetRequiredService<IShipParticularsService>();

            var param = new ShipParticularsParam
            {
                IsAisToggleOn = false,
                IsGPSToggleOn = true,
                ShipKey = "NEW_SHIP_KEY",
                Callsign = "NEW_CALLSIGN",
                ShipName = "NEW_SHIP_NAME",
                ShipType = "FISHING",
                ShipCode = "NEW_SHIP_CODE",
                ShipSatelliteParam = new ShipSatelliteParam
                {
                    SatelliteId = "TEST_SATELLITE_ID",
                    SatelliteType = "KT_SAT"
                }
            };

            // Act
            await sut.Process(param);

            // Assert
            var repository = serviceProvider.GetRequiredService<IShipInfoRepository>();
            var actual = await repository.GetByShipKeyAsync(param.ShipKey);

            actual.Should().NotBeNull();
            actual.IsUseKtsat.Should().BeTrue();
            actual.ShipServices.Should().ContainSingle();
            actual.ShipServices.Should().ContainEquivalentOf(KtSatService(param.ShipKey),
                options => options.Including(s => s.ShipKey)
                                  .Including(s => s.ServiceName)
            );

            actual.ShipSatellite.Should().NotBeNull();
            actual.ShipSatellite.Should().BeEquivalentTo(KtSatellite(1L, param.ShipKey, param.ShipSatelliteParam.SatelliteId),
                options => options.Including(s => s.ShipKey)
                                  .Including(s => s.SatelliteId)
                                  .Including(s => s.SatelliteType)
            );
        }

        [Fact(DisplayName = "신규 ShipInfo가 SK_TELINK를 사용하면 ShipService, ShipSatellite, SkTelinkCompanyShip도 추가된다")]
        public async Task Case5()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var sut = serviceProvider.GetRequiredService<IShipParticularsService>();

            var param = new ShipParticularsParam
            {
                IsAisToggleOn = false,
                IsGPSToggleOn = true,
                ShipKey = "NEW_SHIP_KEY",
                Callsign = "NEW_CALLSIGN",
                ShipName = "NEW_SHIP_NAME",
                ShipType = "FISHING",
                ShipCode = "NEW_SHIP_CODE",
                ShipSatelliteParam = new ShipSatelliteParam
                {
                    SatelliteId = "TEST_SATELLITE_ID",
                    SatelliteType = "SK_TELINK"
                },
                SkTelinkCompanyShipParam = new SkTelinkCompanyShipParam
                {
                    CompanyName = "TEST_COMPANY_KEY"
                }
            };

            // Act
            await sut.Process(param);

            // Assert
            var repository = serviceProvider.GetRequiredService<IShipInfoRepository>();
            var actual = await repository.GetByShipKeyAsync(param.ShipKey);

            actual.Should().NotBeNull();
            actual.IsUseKtsat.Should().BeTrue();
            actual.ExternalShipId.Should().Be(param.ShipSatelliteParam.SatelliteId);

            actual.ShipServices.Should().ContainSingle();
            actual.ShipServices.Should().ContainEquivalentOf(KtSatService(param.ShipKey),
                options => options.Including(s => s.ShipKey)
                                  .Including(s => s.ServiceName)
            );

            actual.ShipSatellite.Should().NotBeNull();
            actual.ShipSatellite.Should().BeEquivalentTo(SkTelinkSatellite(1L, param.ShipKey, param.ShipSatelliteParam.SatelliteId),
                options => options.Including(s => s.ShipKey)
                                  .Including(s => s.SatelliteId)
                                  .Including(s => s.SatelliteType)
            );

            actual.SkTelinkCompanyShip.Should().NotBeNull();
            actual.SkTelinkCompanyShip.CompanyName.Should().Be(param.SkTelinkCompanyShipParam.CompanyName);
        }

        [Fact(DisplayName = "기존 ShipInfo 컬럼 정보를 업데이트한다.")]
        public async Task Case6()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            await serviceProvider.SeedDataAsync(ShipInfo()
                    .WithId(1L)
                    .WithShipKey("UNIQUE_SHIP_KEY")
                    .WithCallsign("OLD_CALLSIGN")
                    .WithShipName("OLD_SHIP_NAME")
                    .WithShipType(ShipTypes.Fishing)
                    .WithShipCode("OLD_SHIP_CODE")
                    .Build());

            var param = new ShipParticularsParam
            {
                IsAisToggleOn = false,
                IsGPSToggleOn = false,
                ShipKey = "UNIQUE_SHIP_KEY",
                Callsign = "UPDATE_CALLSIGN",
                ShipName = "UPDATE_SHIP_NAME",
                ShipType = "PASSENGER",
                ShipCode = "UPDATE_SHIP_CODE"
            };

            var sut = serviceProvider.GetRequiredService<IShipParticularsService>();

            // Act
            await sut.Process(param);

            // Assert
            var repository = serviceProvider.GetRequiredService<IShipInfoRepository>();
            var actual = await repository.GetByShipKeyAsync(param.ShipKey);

            actual.Should().NotBeNull();
            actual.ShipKey.Should().Be(param.ShipKey);
            actual.Callsign.Should().Be(param.Callsign);
            actual.ShipName.Should().Be(param.ShipName);
            actual.ShipType.Should().Be(ShipTypes.Passenger);
            actual.ShipCode.Should().Be(param.ShipCode);

            actual.ShipServices.Should().BeEmpty();

            actual.ShipSatellite.Should().BeNull();

            actual.SkTelinkCompanyShip.Should().BeNull();
        }

        // TODO. 의미가 있을까..
        [Fact(DisplayName = "'sat-ais' ShipService 사용 중일때, AIS Toggle On해도 아무것도 하지 않는다")]
        public async Task Case8()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var param = new ShipParticularsParam
            {
                IsAisToggleOn = true,
                IsGPSToggleOn = false,
                ShipKey = "UNIQUE_SHIP_KEY",
                Callsign = "TEST_CALLSIGN",
                ShipName = "TEST_SHIP_NAME",
                ShipType = "FISHING",
                ShipCode = "TEST_SHIP_CODE",
            };

            await serviceProvider.SeedDataAsync(ShipInfo()
                   .WithShipKey(param.ShipKey)
                   .WithCallsign(param.Callsign)
                   .WithShipName(param.ShipName)
                   .WithShipType(ShipTypes.Fishing)
                   .WithShipCode(param.ShipCode)
                   .WithShipServices(SatAisService(param.ShipKey))
                   .WithIsUseAis(true)
                   .Build());

            var sut = serviceProvider.GetRequiredService<IShipParticularsService>();

            // Act
            await sut.Process(param);

            // Assert
            var repository = serviceProvider.GetRequiredService<IShipInfoRepository>();
            var actual = await repository.GetByShipKeyAsync(param.ShipKey);

            actual.Should().NotBeNull();
            actual.ShipKey.Should().Be(param.ShipKey);
            actual.Callsign.Should().Be(param.Callsign);
            actual.ShipName.Should().Be(param.ShipName);
            actual.ShipType.Should().Be(ShipTypes.Fishing);
            actual.ShipCode.Should().Be(param.ShipCode);

            actual.ShipServices.Should().ContainSingle()
                .And.ContainEquivalentOf(
                    SatAisService(param.ShipKey),
                    options => options.Including(s => s.ShipKey)
                                      .Including(s => s.ServiceName)
            );
        }

        [Fact(DisplayName = "'sat-ais' 서비스만 사용 중이고, AIS Toggle Off하는 경우 ShipServices가 빈다")]
        public async Task Case9()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var param = new ShipParticularsParam
            {
                IsAisToggleOn = false,
                IsGPSToggleOn = false,
                ShipKey = "UNIQUE_SHIP_KEY",
                Callsign = "TEST_CALLSIGN",
                ShipName = "TEST_SHIP_NAME",
                ShipType = "FISHING",
                ShipCode = "TEST_SHIP_CODE",
            };

            await serviceProvider.SeedDataAsync(ShipInfo()
                   .WithShipKey(param.ShipKey)
                   .WithCallsign(param.Callsign)
                   .WithShipName(param.ShipName)
                   .WithShipType(ShipTypes.Fishing)
                   .WithShipCode(param.ShipCode)
                   .WithShipServices(SatAisService(param.ShipKey))
                   .WithIsUseAis(true)
                   .Build());

            var sut = serviceProvider.GetRequiredService<IShipParticularsService>();

            // Act
            await sut.Process(param);

            // Assert
            var repository = serviceProvider.GetRequiredService<IShipInfoRepository>();
            var actual = await repository.GetByShipKeyAsync(param.ShipKey);

            actual.Should().NotBeNull();
            actual.IsUseAis.Should().BeFalse();

            actual.ShipServices.Should().BeEmpty();
        }

        // TODO. 의미가 있을까..
        [Fact(DisplayName = "등록된 ShipService가 없는 경우, AIS Toggle Off해도 아무것도 하지 않는다")]
        public async Task Case10()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var param = new ShipParticularsParam
            {
                IsAisToggleOn = false,
                IsGPSToggleOn = false,
                ShipKey = "UNIQUE_SHIP_KEY",
                Callsign = "TEST_CALLSIGN",
                ShipName = "TEST_SHIP_NAME",
                ShipType = "FISHING",
                ShipCode = "TEST_SHIP_CODE",
            };

            await serviceProvider.SeedDataAsync(ShipInfo()
                   .WithShipKey(param.ShipKey)
                   .WithCallsign(param.Callsign)
                   .WithShipName(param.ShipName)
                   .WithShipType(ShipTypes.Fishing)
                   .WithShipCode(param.ShipCode)
                   .Build());

            var sut = serviceProvider.GetRequiredService<IShipParticularsService>();

            // Act
            await sut.Process(param);

            // Assert
            var repository = serviceProvider.GetRequiredService<IShipInfoRepository>();
            var actual = await repository.GetByShipKeyAsync(param.ShipKey);

            actual.Should().NotBeNull();
            actual.ShipKey.Should().Be(param.ShipKey);
            actual.ShipType.Should().Be(ShipTypes.Fishing);
            actual.IsUseAis.Should().BeFalse();

            actual.ShipServices.Should().BeEmpty();
        }

        // TODO. AIS/GPS Toggle Off 하는 경우와 같지 않나 싶다.
        [Fact(DisplayName = "등록된 ShipService가 없는 경우, GPS Toggle Off 해도 아무것도 하지 않는다")]
        public async Task Case11()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var param = new ShipParticularsParam
            {
                IsAisToggleOn = false,
                IsGPSToggleOn = false,
                ShipKey = "UNIQUE_SHIP_KEY",
                Callsign = "TEST_CALLSIGN",
                ShipName = "TEST_SHIP_NAME",
                ShipType = "FISHING",
                ShipCode = "TEST_SHIP_CODE",
            };

            await serviceProvider.SeedDataAsync(ShipInfo()
                   .WithShipKey(param.ShipKey)
                   .WithCallsign(param.Callsign)
                   .WithShipName(param.ShipName)
                   .WithShipType(ShipTypes.Fishing)
                   .WithShipCode(param.ShipCode)
                   .Build());

            var sut = serviceProvider.GetRequiredService<IShipParticularsService>();

            // Act
            await sut.Process(param);

            // Assert
            var repository = serviceProvider.GetRequiredService<IShipInfoRepository>();
            var actual = await repository.GetByShipKeyAsync(param.ShipKey);

            actual.Should().NotBeNull();
            actual.ShipKey.Should().Be(param.ShipKey);
            actual.ShipType.Should().Be(ShipTypes.Fishing);
            actual.IsUseKtsat.Should().BeFalse();

            actual.ShipServices.Should().BeEmpty();

            actual.ShipSatellite.Should().BeNull();

            actual.SkTelinkCompanyShip.Should().BeNull();
        }


        [Fact(DisplayName = "SK_TELINK 사용중인데 GPS Toggle Off하면 관련 엔티티와 필드를 초기화한다")]
        public async Task Case12()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var param = new ShipParticularsParam
            {
                IsAisToggleOn = false,
                IsGPSToggleOn = false,
                ShipKey = "UNIQUE_SHIP_KEY",
                Callsign = "TEST_CALLSIGN",
                ShipName = "TEST_SHIP_NAME",
                ShipType = "FISHING",
                ShipCode = "TEST_SHIP_CODE",
            };

            await serviceProvider.SeedDataAsync(ShipInfo()
                   .WithShipKey(param.ShipKey)
                   .WithCallsign(param.Callsign)
                   .WithShipName(param.ShipName)
                   .WithShipType(ShipTypes.Fishing)
                   .WithShipCode(param.ShipCode)
                   .WithShipServices(KtSatService(param.ShipKey))
                   .WithShipSatellite(SkTelinkSatellite(param.ShipKey, "SATELLITE_ID"))
                   .WithExternalShipId("SATELLITE_ID")
                   .WithIsUseKtsat(true)
                   .WithSkTelinkCompanyShip(SkTelinkCompanyShip(param.ShipKey, "UNIQUE_COMPANY_NAME"))
                   .Build());

            var sut = serviceProvider.GetRequiredService<IShipParticularsService>();

            // Act
            await sut.Process(param);

            // Assert
            var repository = serviceProvider.GetRequiredService<IShipInfoRepository>();
            var actual = await repository.GetByShipKeyAsync(param.ShipKey);

            actual.Should().NotBeNull();
            actual.ShipKey.Should().Be(param.ShipKey);
            actual.IsUseKtsat.Should().BeFalse();
            actual.ExternalShipId.Should().BeNull();

            actual.ShipServices.Should().BeEmpty();

            actual.ShipSatellite.Should().BeNull();

            actual.SkTelinkCompanyShip.Should().BeNull();
        }

        [Fact(DisplayName = "KT_SAT 서비스를 신규 사용하는 경우, ShipService와 ShipSatellite가 등록된다")]
        public async Task Case13()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var param = new ShipParticularsParam
            {
                IsAisToggleOn = false,
                IsGPSToggleOn = true,
                ShipKey = "UNIQUE_SHIP_KEY",
                Callsign = "TEST_CALLSIGN",
                ShipName = "TEST_SHIP_NAME",
                ShipType = "FISHING",
                ShipCode = "TEST_SHIP_CODE",
                ShipSatelliteParam = new ShipSatelliteParam
                {
                    SatelliteId = "SATELLITE_ID",
                    SatelliteType = "KT_SAT"
                },
            };

            await serviceProvider.SeedDataAsync(ShipInfo()
                   .WithShipKey(param.ShipKey)
                   .WithCallsign(param.Callsign)
                   .WithShipName(param.ShipName)
                   .WithShipType(ShipTypes.Fishing)
                   .WithShipCode(param.ShipCode)
                   .Build());

            var sut = serviceProvider.GetRequiredService<IShipParticularsService>();

            // Act
            await sut.Process(param);

            // Assert
            var repository = serviceProvider.GetRequiredService<IShipInfoRepository>();
            var actual = await repository.GetByShipKeyAsync(param.ShipKey);

            actual.Should().NotBeNull();
            actual.IsUseKtsat.Should().BeTrue();
            actual.ExternalShipId.Should().Be(param.ShipSatelliteParam.SatelliteId);

            actual.ShipServices.Should().ContainSingle()
                .And.ContainEquivalentOf(
                    KtSatService(param.ShipKey),
                    options => options.Including(s => s.ShipKey)
                                      .Including(s => s.ServiceName)
            );

            actual.ShipSatellite.Should().NotBeNull();
            actual.ShipSatellite.Should().BeEquivalentTo(
                KtSatellite(param.ShipKey, param.ShipSatelliteParam.SatelliteId),
                options => options.Including(s => s.ShipKey)
                                  .Including(s => s.SatelliteId)
                                  .Including(s => s.SatelliteType)
            );

            actual.SkTelinkCompanyShip.Should().BeNull();
        }

        [Fact(DisplayName = "SK_TELINK 서비스를 신규 사용하는 경우, ShipService, ShipSatellite, SkTelinkCompanyShip가 등록된다")]
        public async Task Case14()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var param = new ShipParticularsParam
            {
                IsAisToggleOn = false,
                IsGPSToggleOn = true,
                ShipKey = "UNIQUE_SHIP_KEY",
                Callsign = "TEST_CALLSIGN",
                ShipName = "TEST_SHIP_NAME",
                ShipType = "FISHING",
                ShipCode = "TEST_SHIP_CODE",
                ShipSatelliteParam = new ShipSatelliteParam
                {
                    SatelliteId = "SATELLITE_ID",
                    SatelliteType = "SK_TELINK"
                },
                SkTelinkCompanyShipParam = new SkTelinkCompanyShipParam
                {
                    CompanyName = "UNIQUE_COMPANY_NAME"
                }
            };

            await serviceProvider.SeedDataAsync(ShipInfo()
                   .WithShipKey(param.ShipKey)
                   .WithCallsign(param.Callsign)
                   .WithShipName(param.ShipName)
                   .WithShipType(ShipTypes.Fishing)
                   .WithShipCode(param.ShipCode)
                   .Build());

            var sut = serviceProvider.GetRequiredService<IShipParticularsService>();

            // Act
            await sut.Process(param);

            // Assert
            var repository = serviceProvider.GetRequiredService<IShipInfoRepository>();
            var actual = await repository.GetByShipKeyAsync(param.ShipKey);

            actual.Should().NotBeNull();
            actual.IsUseKtsat.Should().BeTrue();
            actual.ExternalShipId.Should().Be(param.ShipSatelliteParam.SatelliteId);

            actual.ShipServices.Should().ContainSingle()
                .And.ContainEquivalentOf(
                    KtSatService(param.ShipKey),
                    options => options.Including(s => s.ShipKey)
                                      .Including(s => s.ServiceName)
            );

            actual.ShipSatellite.Should().NotBeNull();
            actual.ShipSatellite.Should().BeEquivalentTo(
                SkTelinkSatellite(param.ShipKey, param.ShipSatelliteParam.SatelliteId),
                options => options.Including(s => s.ShipKey)
                                  .Including(s => s.SatelliteId)
                                  .Including(s => s.SatelliteType)
            );

            actual.SkTelinkCompanyShip.Should().NotBeNull();
            actual.SkTelinkCompanyShip.CompanyName.Should()
                .Be(param.SkTelinkCompanyShipParam.CompanyName);
        }

        [Fact(DisplayName = "KT_SAT에서 SK_TELINK로 서비스 변경하는 경우, ShipSatellite가 업데이트되고, SKTelinkCompanyShip이 추가된다")]
        public async Task Case15()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var param = new ShipParticularsParam
            {
                IsAisToggleOn = false,
                IsGPSToggleOn = true,
                ShipKey = "UNIQUE_SHIP_KEY",
                Callsign = "TEST_CALLSIGN",
                ShipName = "TEST_SHIP_NAME",
                ShipType = "FISHING",
                ShipCode = "TEST_SHIP_CODE",
                ShipSatelliteParam = new ShipSatelliteParam
                {
                    SatelliteId = "SATELLITE_ID",
                    SatelliteType = "SK_TELINK"
                },
                SkTelinkCompanyShipParam = new SkTelinkCompanyShipParam
                {
                    CompanyName = "UNIQUE_COMPANY_NAME"
                }
            };

            await serviceProvider.SeedDataAsync(ShipInfo()
                   .WithShipKey(param.ShipKey)
                   .WithCallsign(param.Callsign)
                   .WithShipName(param.ShipName)
                   .WithShipType(ShipTypes.Fishing)
                   .WithShipCode(param.ShipCode)
                   .WithShipServices(KtSatService(param.ShipKey))
                   .WithShipSatellite(KtSatellite(param.ShipKey, "SATELLITE_ID"))
                   .WithExternalShipId("SATELLITE_ID")
                   .WithIsUseKtsat(true)
                   .Build());

            var sut = serviceProvider.GetRequiredService<IShipParticularsService>();

            // Act
            await sut.Process(param);

            // Assert
            var repository = serviceProvider.GetRequiredService<IShipInfoRepository>();
            var actual = await repository.GetByShipKeyAsync(param.ShipKey);

            actual.Should().NotBeNull();
            actual.IsUseKtsat.Should().BeTrue();
            actual.ExternalShipId.Should().Be(param.ShipSatelliteParam.SatelliteId);

            actual.ShipServices.Should().ContainSingle()
                .And.ContainEquivalentOf(
                    KtSatService(param.ShipKey),
                    options => options.Including(s => s.ShipKey)
                                      .Including(s => s.ServiceName)
            );

            actual.ShipSatellite.Should().NotBeNull();
            actual.ShipSatellite.Should().BeEquivalentTo(
                SkTelinkSatellite(param.ShipKey, param.ShipSatelliteParam.SatelliteId),
                options => options.Including(s => s.ShipKey)
                                  .Including(s => s.SatelliteId)
                                  .Including(s => s.SatelliteType)
            );

            actual.SkTelinkCompanyShip.Should().NotBeNull();
            actual.SkTelinkCompanyShip.CompanyName.Should()
                .Be(param.SkTelinkCompanyShipParam.CompanyName);
        }


        [Fact(DisplayName = "SK_TELINK에서 KT_SAT로 서비스 변경하는 경우, ShipSatellite가 업데이트되고, SKTelinkCompanyShip이 제거된다")]
        public async Task Case16()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var param = new ShipParticularsParam
            {
                IsAisToggleOn = false,
                IsGPSToggleOn = true,
                ShipKey = "UNIQUE_SHIP_KEY",
                Callsign = "TEST_CALLSIGN",
                ShipName = "TEST_SHIP_NAME",
                ShipType = "FISHING",
                ShipCode = "TEST_SHIP_CODE",
                ShipSatelliteParam = new ShipSatelliteParam
                {
                    SatelliteId = "SATELLITE_ID",
                    SatelliteType = "KT_SAT"
                }
            };

            await serviceProvider.SeedDataAsync(ShipInfo()
                   .WithShipKey(param.ShipKey)
                   .WithCallsign(param.Callsign)
                   .WithShipName(param.ShipName)
                   .WithShipType(ShipTypes.Fishing)
                   .WithShipCode(param.ShipCode)
                   .WithShipServices(KtSatService(param.ShipKey))
                   .WithShipSatellite(SkTelinkSatellite(param.ShipKey, "SATELLITE_ID"))
                   .WithExternalShipId("SATELLITE_ID")
                   .WithIsUseKtsat(true)
                   .WithSkTelinkCompanyShip(SkTelinkCompanyShip(param.ShipKey, "UNIQUE_COMPANY_NAME"))
                   .Build());

            var sut = serviceProvider.GetRequiredService<IShipParticularsService>();

            // Act
            await sut.Process(param);

            // Assert
            var repository = serviceProvider.GetRequiredService<IShipInfoRepository>();
            var actual = await repository.GetByShipKeyAsync(param.ShipKey);

            actual.Should().NotBeNull();
            actual.IsUseKtsat.Should().BeTrue();
            actual.ExternalShipId.Should().Be(param.ShipSatelliteParam.SatelliteId);

            actual.ShipServices.Should().ContainSingle()
                .And.ContainEquivalentOf(
                    KtSatService(param.ShipKey),
                    options => options.Including(s => s.ShipKey)
                                      .Including(s => s.ServiceName)
            );

            actual.ShipSatellite.Should().NotBeNull();
            actual.ShipSatellite.Should().BeEquivalentTo(
                KtSatellite(param.ShipKey, param.ShipSatelliteParam.SatelliteId),
                options => options.Including(s => s.ShipKey)
                                  .Including(s => s.SatelliteId)
                                  .Including(s => s.SatelliteType)
            );

            actual.SkTelinkCompanyShip.Should().BeNull();
        }

        [Fact(DisplayName = "SK TELINK의 CompanyName을 업데이트 한다")]
        public async Task Case17()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var param = new ShipParticularsParam
            {
                IsAisToggleOn = false,
                IsGPSToggleOn = true,
                ShipKey = "UNIQUE_SHIP_KEY",
                Callsign = "TEST_CALLSIGN",
                ShipName = "TEST_SHIP_NAME",
                ShipType = "FISHING",
                ShipCode = "TEST_SHIP_CODE",
                ShipSatelliteParam = new ShipSatelliteParam
                {
                    SatelliteId = "SATELLITE_ID",
                    SatelliteType = "SK_TELINK"
                },
                SkTelinkCompanyShipParam = new SkTelinkCompanyShipParam
                {
                    CompanyName = "UPDATE_COMPANY_NAME"
                }
            };

            await serviceProvider.SeedDataAsync(ShipInfo()
                   .WithShipKey(param.ShipKey)
                   .WithCallsign(param.Callsign)
                   .WithShipName(param.ShipName)
                   .WithShipType(ShipTypes.Fishing)
                   .WithShipCode(param.ShipCode)
                   .WithShipServices(KtSatService(param.ShipKey))
                   .WithShipSatellite(SkTelinkSatellite(param.ShipKey, "SATELLITE_ID"))
                   .WithExternalShipId("SATELLITE_ID")
                   .WithIsUseKtsat(true)
                   .WithSkTelinkCompanyShip(SkTelinkCompanyShip(param.ShipKey, "UNIQUE_COMPANY_NAME"))
                   .Build());

            var sut = serviceProvider.GetRequiredService<IShipParticularsService>();

            // Act
            await sut.Process(param);

            // Assert
            var repository = serviceProvider.GetRequiredService<IShipInfoRepository>();
            var actual = await repository.GetByShipKeyAsync(param.ShipKey);

            actual.Should().NotBeNull();

            actual.SkTelinkCompanyShip.Should().NotBeNull();
            actual.SkTelinkCompanyShip.CompanyName.Should()
                .Be(param.SkTelinkCompanyShipParam.CompanyName);
        }

        [Fact(DisplayName = "SK TELINK의 CompanyName 업데이트시 다른 엔티티는 변경되지 않는다")]
        public async Task Case18()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var param = new ShipParticularsParam
            {
                IsAisToggleOn = false,
                IsGPSToggleOn = true,
                ShipKey = "UNIQUE_SHIP_KEY",
                Callsign = "TEST_CALLSIGN",
                ShipName = "TEST_SHIP_NAME",
                ShipType = "FISHING",
                ShipCode = "TEST_SHIP_CODE",
                ShipSatelliteParam = new ShipSatelliteParam
                {
                    SatelliteId = "SATELLITE_ID",
                    SatelliteType = "SK_TELINK"
                },
                SkTelinkCompanyShipParam = new SkTelinkCompanyShipParam
                {
                    CompanyName = "UPDATE_COMPANY_NAME"
                }
            };

            await serviceProvider.SeedDataAsync(ShipInfo()
                   .WithShipKey(param.ShipKey)
                   .WithCallsign(param.Callsign)
                   .WithShipName(param.ShipName)
                   .WithShipType(ShipTypes.Fishing)
                   .WithShipCode(param.ShipCode)
                   .WithShipServices(KtSatService(param.ShipKey))
                   .WithShipSatellite(SkTelinkSatellite(param.ShipKey, "SATELLITE_ID"))
                   .WithExternalShipId("SATELLITE_ID")
                   .WithIsUseKtsat(true)
                   .WithSkTelinkCompanyShip(SkTelinkCompanyShip(param.ShipKey, "UNIQUE_COMPANY_NAME"))
                   .Build());

            var sut = serviceProvider.GetRequiredService<IShipParticularsService>();

            // Act
            await sut.Process(param);

            // Assert
            var repository = serviceProvider.GetRequiredService<IShipInfoRepository>();
            var actual = await repository.GetByShipKeyAsync(param.ShipKey);

            actual.Should().NotBeNull();

            actual.ShipServices.Should()
               .ContainEquivalentOf(
                    KtSatService(param.ShipKey),
                    options => options.Including(s => s.ShipKey).Including(s => s.ServiceName)
               );
            actual.ShipSatellite.Should()
                .BeEquivalentTo(
                    SkTelinkSatellite(param.ShipKey, param.ShipSatelliteParam.SatelliteId),
                    options => options.Including(s => s.ShipKey)
                                      .Including(s => s.SatelliteId)
                                      .Including(s => s.SatelliteType)
                );
        }
    }
}
