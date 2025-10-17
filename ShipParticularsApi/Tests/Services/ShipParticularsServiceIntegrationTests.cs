using FluentAssertions;
using ShipParticularsApi.Contexts;
using ShipParticularsApi.Entities;
using ShipParticularsApi.Repositories;
using ShipParticularsApi.Services;
using ShipParticularsApi.Services.Dtos;
using Xunit;
using Xunit.Abstractions;

// NOTE. TransactionDecorator (sut) 획득하는 코드 중복
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
            var sut = serviceProvider.GetRequiredService<IShipParticularsService>(); // TransactionDecorator

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
            var actual = await repository.GetByShipKeyAsync("NEW_SHIP_KEY");

            actual.Should().NotBeNull();
            actual.IsUseAis.Should().BeTrue();
            actual.ShipServices.Should().ContainSingle();
        }
    }
}

