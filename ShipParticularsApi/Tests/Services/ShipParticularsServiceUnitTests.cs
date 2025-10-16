using FluentAssertions;
using Moq;
using ShipParticularsApi.Entities;
using ShipParticularsApi.Services;
using ShipParticularsApi.Services.Dtos;
using Xunit;
using static ShipParticularsApi.Tests.Builders.ShipInfoTestBuilder;
using static ShipParticularsApi.Tests.Builders.ShipSatelliteTestBuilder;
using static ShipParticularsApi.Tests.Builders.ShipServiceTestBuilder;
using static ShipParticularsApi.Tests.Builders.SkTelinkCompanyShipTestBuilder;
namespace ShipParticularsApi.Tests.Services
{
    public class ShipParticularsServiceUnitTests
    {
        private readonly ShipParticularsService _sut;
        private readonly Mock<IShipInfoRepository> _mockShipInfoRepository;

        public ShipParticularsServiceUnitTests()
        {
            _mockShipInfoRepository = new Mock<IShipInfoRepository>();
            _sut = new ShipParticularsService(_mockShipInfoRepository.Object);
        }

        [Fact(DisplayName = "신규 ShipInfo이고, AIS 토글이 Off인 경우 ShipServices가 비어있다.")]
        public async Task Case1()
        {
            // Arrange
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

            _mockShipInfoRepository
                .Setup(e => e.GetByShipKeyAsync(param.ShipKey))
                .ReturnsAsync((ShipInfo?)null);

            ShipInfo? capturedEntity = null;
            _mockShipInfoRepository.Setup(e => e.UpsertAsync(It.IsAny<ShipInfo>()))
                .Callback<ShipInfo>(arg => capturedEntity = arg)
                .ReturnsAsync((ShipInfo entity) => entity);

            // Act
            await _sut.Process(param);

            // Assert
            _mockShipInfoRepository.Verify(e => e.UpsertAsync(It.IsAny<ShipInfo>()), Times.Once);

            capturedEntity.Should().NotBeNull();
            capturedEntity.Id.Should().Be(0L);
            capturedEntity.IsUseAis.Should().BeFalse();
            capturedEntity.ShipServices.Should().BeEmpty();
        }

        [Fact(DisplayName = "신규 ShipInfo이고, AIS 토글이 On인 경우 ShipServices의 길이는 1이다")]
        public async Task Case2()
        {
            // Arrange
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

            _mockShipInfoRepository
                .Setup(e => e.GetByShipKeyAsync(param.ShipKey))
                .ReturnsAsync((ShipInfo?)null);

            ShipInfo? capturedEntity = null;
            _mockShipInfoRepository.Setup(e => e.UpsertAsync(It.IsAny<ShipInfo>()))
                .Callback<ShipInfo>(arg => capturedEntity = arg)
                .ReturnsAsync((ShipInfo entity) => entity);

            // Act
            await _sut.Process(param);

            // Assert
            _mockShipInfoRepository.Verify(e => e.UpsertAsync(It.IsAny<ShipInfo>()), Times.Once);

            capturedEntity.Should().NotBeNull();
            capturedEntity.Id.Should().Be(0L);
            capturedEntity.IsUseAis.Should().BeTrue();
            capturedEntity.ShipServices.Should().HaveCount(1);
            capturedEntity.ShipServices.Should().ContainEquivalentOf(SatAisService(param.ShipKey));
        }

        [Fact(DisplayName = "신규 ShipInfo이고, GPS Toggle Off인 경우 ShipServices가 비어있다.")]
        public async Task Case3()
        {
            // Arrange
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

            _mockShipInfoRepository
                .Setup(e => e.GetByShipKeyAsync(param.ShipKey))
                .ReturnsAsync((ShipInfo?)null);

            ShipInfo? capturedEntity = null;
            _mockShipInfoRepository.Setup(e => e.UpsertAsync(It.IsAny<ShipInfo>()))
                .Callback<ShipInfo>(arg => capturedEntity = arg)
                .ReturnsAsync((ShipInfo entity) => entity);

            // Act
            await _sut.Process(param);

            // Assert
            _mockShipInfoRepository.Verify(e => e.UpsertAsync(It.IsAny<ShipInfo>()), Times.Once);

            capturedEntity.Should().NotBeNull();
            capturedEntity.Id.Should().Be(0L);

            capturedEntity.ShipServices.Should().BeEmpty();
            capturedEntity.ShipSatellite.Should().BeNull();
            capturedEntity.SkTelinkCompanyShip.Should().BeNull();
        }

        [Fact(DisplayName = "신규 ShipInfo이고, GPS Toggle On & SatelliteType이 SK가 아닌 경우 ShipService, ShipSatellite만 추가된다")]
        public async Task Case4()
        {
            // Arrange
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

            _mockShipInfoRepository
                .Setup(e => e.GetByShipKeyAsync(param.ShipKey))
                .ReturnsAsync((ShipInfo?)null);

            ShipInfo? capturedEntity = null;
            _mockShipInfoRepository.Setup(e => e.UpsertAsync(It.IsAny<ShipInfo>()))
                .Callback<ShipInfo>(arg => capturedEntity = arg)
                .ReturnsAsync((ShipInfo entity) => entity);

            // Act
            await _sut.Process(param);

            // Assert
            _mockShipInfoRepository.Verify(e => e.UpsertAsync(It.IsAny<ShipInfo>()), Times.Once);

            capturedEntity.Should().NotBeNull();
            capturedEntity.Id.Should().Be(0L);
            capturedEntity.ExternalShipId.Should().BeSameAs(param.ShipSatelliteParam.SatelliteId);
            capturedEntity.IsUseKtsat.Should().BeTrue();

            capturedEntity.ShipServices.Should().HaveCount(1)
                .And.ContainEquivalentOf(KtSatService(param.ShipKey));

            capturedEntity.ShipSatellite.Should().NotBeNull()
                .And.BeEquivalentTo(KtSatellite(param.ShipKey, param.ShipSatelliteParam.SatelliteId),
                options => options.Excluding(s => s.UpdateDateTime));

            capturedEntity.SkTelinkCompanyShip.Should().BeNull();
        }

        [Fact(DisplayName = "신규 ShipInfo이고, GPS Toggle On & SatelliteType이 SK인 경우 ShipService, ShipSatellite, SkTelinkCompanyShip이 추가된다")]
        public async Task Case5()
        {
            // Arrange
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

            _mockShipInfoRepository
                .Setup(e => e.GetByShipKeyAsync(param.ShipKey))
                .ReturnsAsync((ShipInfo?)null);

            ShipInfo? capturedEntity = null;
            _mockShipInfoRepository.Setup(e => e.UpsertAsync(It.IsAny<ShipInfo>()))
                .Callback<ShipInfo>(arg => capturedEntity = arg)
                .ReturnsAsync((ShipInfo entity) => entity);

            // Act
            await _sut.Process(param);

            // Assert
            _mockShipInfoRepository.Verify(e => e.UpsertAsync(It.IsAny<ShipInfo>()), Times.Once);

            capturedEntity.Should().NotBeNull();
            capturedEntity.Id.Should().Be(0L);
            capturedEntity.ExternalShipId.Should().BeSameAs(param.ShipSatelliteParam.SatelliteId);
            capturedEntity.IsUseKtsat.Should().BeTrue();

            capturedEntity.ShipServices.Should().HaveCount(1)
                .And.ContainEquivalentOf(KtSatService(param.ShipKey));

            capturedEntity.ShipSatellite.Should().NotBeNull()
                .And.BeEquivalentTo(SkTelinkSatellite(param.ShipKey, param.ShipSatelliteParam.SatelliteId),
                options => options.Excluding(s => s.UpdateDateTime));

            capturedEntity.SkTelinkCompanyShip.Should().NotBeNull()
                .And.BeEquivalentTo(SkTelinkCompanyShip(param.ShipKey, param.SkTelinkCompanyShipParam.CompanyName));
        }

        [Fact(DisplayName = "기존 ShipInfo 컬럼 정보를 업데이트한다.")]
        public async Task Case6()
        {
            // Arrange
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

            var oldShipInfo = ShipInfo()
                    .WithId(1L)
                    .WithShipKey("UNIQUE_SHIP_KEY")
                    .WithCallsign("OLD_CALLSIGN")
                    .WithShipName("OLD_SHIP_NAME")
                    .WithShipType(ShipTypes.Fishing)
                    .WithShipCode("OLD_SHIP_CODE")
                    .Build();
            _mockShipInfoRepository
                .Setup(e => e.GetByShipKeyAsync(param.ShipKey))
                .ReturnsAsync(oldShipInfo);

            ShipInfo? capturedEntity = null;
            _mockShipInfoRepository.Setup(e => e.UpsertAsync(It.IsAny<ShipInfo>()))
                .Callback<ShipInfo>(arg => capturedEntity = arg)
                .ReturnsAsync((ShipInfo entity) => entity);

            // Act
            await _sut.Process(param);

            // Assert
            capturedEntity.Should().NotBeNull();
            capturedEntity.Should().BeEquivalentTo(
                    ShipInfo().WithId(1L)
                        .WithShipKey(param.ShipKey)
                        .WithCallsign(param.Callsign)
                        .WithShipName(param.ShipName)
                        .WithShipType(ShipTypes.Passenger)
                        .WithShipCode(param.ShipCode)
                        .Build()
                );
        }

        [Fact(DisplayName = "AIS Toggle On인 경우 서비스('sat-ais')가 추가된다")]
        public async Task Case7()
        {
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

            var oldShipInfo = ShipInfo()
                    .WithId(1L)
                    .WithShipKey("UNIQUE_SHIP_KEY")
                    .Build();
            _mockShipInfoRepository
                .Setup(e => e.GetByShipKeyAsync(param.ShipKey))
                .ReturnsAsync(oldShipInfo);

            ShipInfo? capturedEntity = null;
            _mockShipInfoRepository.Setup(e => e.UpsertAsync(It.IsAny<ShipInfo>()))
                .Callback<ShipInfo>(arg => capturedEntity = arg)
                .ReturnsAsync((ShipInfo entity) => entity);

            // Act
            await _sut.Process(param);

            // Assert
            capturedEntity.Should().NotBeNull();
            capturedEntity.ShipServices.Should().NotBeEmpty();
            capturedEntity.ShipServices.Should()
                .ContainEquivalentOf(SatAisService(param.ShipKey));
            capturedEntity.IsUseAis.Should().BeTrue();
        }

        [Fact(DisplayName = "'sat-ais' ShipService 사용 중일때, AIS Toggle On해도 아무것도 하지 않는다")]
        public async Task Case8()
        {
            // Arrange
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

            var oldShipInfo = ShipInfo()
                   .WithId(1L)
                   .WithShipKey(param.ShipKey)
                   .WithCallsign(param.Callsign)
                   .WithShipName(param.ShipName)
                   .WithShipType(ShipTypes.Fishing)
                   .WithShipCode(param.ShipCode)
                   .WithShipServices(SatAisService(param.ShipKey))
                   .WithIsUseAis(true)
                   .Build();
            _mockShipInfoRepository
                .Setup(e => e.GetByShipKeyAsync(param.ShipKey))
                .ReturnsAsync(oldShipInfo);

            ShipInfo? capturedEntity = null;
            _mockShipInfoRepository.Setup(e => e.UpsertAsync(It.IsAny<ShipInfo>()))
                .Callback<ShipInfo>(arg => capturedEntity = arg)
                .ReturnsAsync((ShipInfo entity) => entity);

            // Act
            await _sut.Process(param);

            // Assert
            capturedEntity.Should().NotBeNull();
            capturedEntity.Should().BeEquivalentTo(oldShipInfo);
        }

        [Fact(DisplayName = "'sat-ais' ShipService를 사용 중이고, AIS Toggle Off하는 경우 컬렉션 길이가 0이 된다")]
        public async Task Case9()
        {
            // Arrange
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

            var oldShipInfo = ShipInfo()
                   .WithId(1L)
                   .WithShipKey(param.ShipKey)
                   .WithCallsign(param.Callsign)
                   .WithShipName(param.ShipName)
                   .WithShipType(ShipTypes.Fishing)
                   .WithShipCode(param.ShipCode)
                   .WithShipServices(SatAisService(param.ShipKey))
                   .WithIsUseAis(true)
                   .Build();

            _mockShipInfoRepository
                .Setup(e => e.GetByShipKeyAsync(param.ShipKey))
                .ReturnsAsync(oldShipInfo);

            ShipInfo? capturedEntity = null;
            _mockShipInfoRepository.Setup(e => e.UpsertAsync(It.IsAny<ShipInfo>()))
                .Callback<ShipInfo>(arg => capturedEntity = arg)
                .ReturnsAsync((ShipInfo entity) => entity);

            // Act
            await _sut.Process(param);

            // Assert
            capturedEntity.Should().NotBeNull();
            capturedEntity.ShipServices.Should().BeEmpty();
            capturedEntity.IsUseAis.Should().BeFalse();
        }

        // NOTE. Case6에서 검증되는 부분이 아닌가 싶다.
        [Fact(DisplayName = "등록된 ShipService가 없는 경우, AIS Toggle Off해도 아무것도 하지 않는다")]
        public async Task Case10()
        {
            // Arrange
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

            var oldShipInfo = ShipInfo()
                   .WithId(1L)
                   .WithShipKey(param.ShipKey)
                   .WithCallsign(param.Callsign)
                   .WithShipName(param.ShipName)
                   .WithShipType(ShipTypes.Fishing)
                   .WithShipCode(param.ShipCode)
                   .Build();
            _mockShipInfoRepository
                .Setup(e => e.GetByShipKeyAsync(param.ShipKey))
                .ReturnsAsync(oldShipInfo);

            ShipInfo? capturedEntity = null;
            _mockShipInfoRepository.Setup(e => e.UpsertAsync(It.IsAny<ShipInfo>()))
                .Callback<ShipInfo>(arg => capturedEntity = arg)
                .ReturnsAsync((ShipInfo entity) => entity);

            // Act
            await _sut.Process(param);

            capturedEntity.Should().NotBeNull();
            capturedEntity.Should().BeEquivalentTo(oldShipInfo);
        }

        [Fact(DisplayName = "등록된 ShipService가 없는 경우, GPS Toggle Off 해도 아무것도 하지 않는다")]
        public async Task Case11()
        {
            // Arrange
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

            var oldShipInfo = ShipInfo()
                   .WithId(1L)
                   .WithShipKey(param.ShipKey)
                   .WithCallsign(param.Callsign)
                   .WithShipName(param.ShipName)
                   .WithShipType(ShipTypes.Fishing)
                   .WithShipCode(param.ShipCode)
                   .Build();
            _mockShipInfoRepository
                .Setup(e => e.GetByShipKeyAsync(param.ShipKey))
                .ReturnsAsync(oldShipInfo);

            ShipInfo? capturedEntity = null;
            _mockShipInfoRepository.Setup(e => e.UpsertAsync(It.IsAny<ShipInfo>()))
                .Callback<ShipInfo>(arg => capturedEntity = arg)
                .ReturnsAsync((ShipInfo entity) => entity);

            // Act
            await _sut.Process(param);

            capturedEntity.Should().NotBeNull();
            capturedEntity.ShipServices.Should().BeEmpty();

            capturedEntity.ShipSatellite.Should().BeNull();
            // TODO. ShipInfo 단위 테스트에서 초기화시 false가 되는데 왜 null? 
            capturedEntity.IsUseKtsat.Should().BeNull();
            capturedEntity.ExternalShipId.Should().BeNull();

            capturedEntity.SkTelinkCompanyShip.Should().BeNull();
        }

        [Fact(DisplayName = "사용중인 GPS 서비스를 비활성화하면 관련 엔티티와 필드를 초기화한다")]
        public async Task Case12()
        {
            // Arrange
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

            var oldShipInfo = ShipInfo()
                   .WithId(1L)
                   .WithShipKey(param.ShipKey)
                   .WithCallsign(param.Callsign)
                   .WithShipName(param.ShipName)
                   .WithShipType(ShipTypes.Fishing)
                   .WithShipCode(param.ShipCode)
                   .WithShipServices(KtSatService(1L, "UNIQUE_SHIP_KEY"))
                   .WithShipSatellite(SkTelinkSatellite(1L, "UNIQUE_SHIP_KEY", "SATELLITE_ID"))
                   .WithExternalShipId("SATELLITE_ID")
                   .WithIsUseKtsat(true)
                   .WithSkTelinkCompanyShip(SkTelinkCompanyShip(1L, "UNIQUE_SHIP_KEY", "UNIQUE_COMPANY_NAME"))
                   .Build();
            _mockShipInfoRepository
                .Setup(e => e.GetByShipKeyAsync(param.ShipKey))
                .ReturnsAsync(oldShipInfo);

            ShipInfo? capturedEntity = null;
            _mockShipInfoRepository.Setup(e => e.UpsertAsync(It.IsAny<ShipInfo>()))
                .Callback<ShipInfo>(arg => capturedEntity = arg)
                .ReturnsAsync((ShipInfo entity) => entity);

            // Act
            await _sut.Process(param);

            capturedEntity.Should().NotBeNull();

            capturedEntity.ShipServices.Should().BeEmpty();

            capturedEntity.ShipSatellite.Should().BeNull();
            capturedEntity.IsUseKtsat.Should().BeFalse();
            capturedEntity.ExternalShipId.Should().BeNull();

            capturedEntity.SkTelinkCompanyShip.Should().BeNull();
        }

        [Fact(DisplayName = "KT_SAT 서비스를 신규 사용하는 경우, ShipService와 ShipSatellite가 등록된다")]
        public async Task Case13()
        {
            // Arrange
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

            var oldShipInfo = ShipInfo()
                   .WithId(1L)
                   .WithShipKey(param.ShipKey)
                   .WithCallsign(param.Callsign)
                   .WithShipName(param.ShipName)
                   .WithShipType(ShipTypes.Fishing)
                   .WithShipCode(param.ShipCode)
                   .Build();
            _mockShipInfoRepository
                .Setup(e => e.GetByShipKeyAsync(param.ShipKey))
                .ReturnsAsync(oldShipInfo);

            ShipInfo? capturedEntity = null;
            _mockShipInfoRepository.Setup(e => e.UpsertAsync(It.IsAny<ShipInfo>()))
                .Callback<ShipInfo>(arg => capturedEntity = arg)
                .ReturnsAsync((ShipInfo entity) => entity);

            // Act
            await _sut.Process(param);

            // Assert
            capturedEntity.Should().NotBeNull();

            capturedEntity.ShipServices.Should().ContainEquivalentOf(KtSatService("UNIQUE_SHIP_KEY"));

            capturedEntity.ShipSatellite.Should().BeEquivalentTo(KtSatellite("UNIQUE_SHIP_KEY", "SATELLITE_ID"),
                options => options.Excluding(s => s.UpdateDateTime));
            capturedEntity.ExternalShipId.Should().Be("SATELLITE_ID");
            capturedEntity.IsUseKtsat.Should().BeTrue();

            capturedEntity.SkTelinkCompanyShip.Should().BeNull();
        }

        [Fact(DisplayName = "신규로 SK_TELINK 서비스 사용하는 경우, ShipService,ShipSatellite, SkTelinkCompanyShip가 등록된다")]
        public async Task Case14()
        {
            // Arrange
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

            var oldShipInfo = ShipInfo()
                   .WithId(1L)
                   .WithShipKey(param.ShipKey)
                   .WithCallsign(param.Callsign)
                   .WithShipName(param.ShipName)
                   .WithShipType(ShipTypes.Fishing)
                   .WithShipCode(param.ShipCode)
                   .Build();
            _mockShipInfoRepository
                .Setup(e => e.GetByShipKeyAsync(param.ShipKey))
                .ReturnsAsync(oldShipInfo);

            ShipInfo? capturedEntity = null;
            _mockShipInfoRepository.Setup(e => e.UpsertAsync(It.IsAny<ShipInfo>()))
                .Callback<ShipInfo>(arg => capturedEntity = arg)
                .ReturnsAsync((ShipInfo entity) => entity);

            // Act
            await _sut.Process(param);

            // Assert
            capturedEntity.Should().NotBeNull();

            capturedEntity.ShipServices.Should().ContainEquivalentOf(KtSatService("UNIQUE_SHIP_KEY"));

            capturedEntity.ShipSatellite.Should().BeEquivalentTo(SkTelinkSatellite("UNIQUE_SHIP_KEY", "SATELLITE_ID"),
                options => options.Excluding(s => s.UpdateDateTime));
            capturedEntity.ExternalShipId.Should().Be("SATELLITE_ID");
            capturedEntity.IsUseKtsat.Should().BeTrue();

            capturedEntity.SkTelinkCompanyShip.Should().BeEquivalentTo(SkTelinkCompanyShip("UNIQUE_SHIP_KEY", "UNIQUE_COMPANY_NAME"));
        }

        [Fact(DisplayName = "KT_SAT에서 SK_TELINK로 서비스 변경하는 경우, ShipSatellite가 업데이트되고, SKTelinkCompanyShip이 추가된다")]
        public async Task Case15()
        {
            // Arrange
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

            var oldShipInfo = ShipInfo()
                   .WithId(1L)
                   .WithShipKey(param.ShipKey)
                   .WithCallsign(param.Callsign)
                   .WithShipName(param.ShipName)
                   .WithShipType(ShipTypes.Fishing)
                   .WithShipCode(param.ShipCode)
                   .WithShipServices(KtSatService(1L, "UNIQUE_SHIP_KEY"))
                   .WithShipSatellite(KtSatellite(1L, "UNIQUE_SHIP_KEY", "SATELLITE_ID"))
                   .WithExternalShipId("SATELLITE_ID")
                   .WithIsUseKtsat(true)
                   .Build();
            _mockShipInfoRepository
                .Setup(e => e.GetByShipKeyAsync(param.ShipKey))
                .ReturnsAsync(oldShipInfo);

            ShipInfo? capturedEntity = null;
            _mockShipInfoRepository.Setup(e => e.UpsertAsync(It.IsAny<ShipInfo>()))
                .Callback<ShipInfo>(arg => capturedEntity = arg)
                .ReturnsAsync((ShipInfo entity) => entity);

            // Act
            await _sut.Process(param);

            // Assert
            capturedEntity.Should().NotBeNull();

            capturedEntity.ShipServices.Should().ContainEquivalentOf(KtSatService(1L, "UNIQUE_SHIP_KEY"));

            capturedEntity.ShipSatellite.Should().BeEquivalentTo(SkTelinkSatellite(1L, "UNIQUE_SHIP_KEY", "SATELLITE_ID"),
                options => options.Excluding(s => s.UpdateDateTime));
            capturedEntity.ExternalShipId.Should().Be("SATELLITE_ID");
            capturedEntity.IsUseKtsat.Should().BeTrue();

            capturedEntity.SkTelinkCompanyShip.Should().BeEquivalentTo(SkTelinkCompanyShip("UNIQUE_SHIP_KEY", "UNIQUE_COMPANY_NAME"));
        }

        [Fact(DisplayName = "SK_TELINK에서 KT_SAT로 서비스 변경하는 경우, ShipSatellite가 업데이트되고, SKTelinkCompanyShip이 제거된다")]
        public async Task Case16()
        {
            // Arrange
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

            var oldShipInfo = ShipInfo()
                   .WithId(1L)
                   .WithShipKey(param.ShipKey)
                   .WithCallsign(param.Callsign)
                   .WithShipName(param.ShipName)
                   .WithShipType(ShipTypes.Fishing)
                   .WithShipCode(param.ShipCode)
                   .WithShipServices(KtSatService(1L, "UNIQUE_SHIP_KEY"))
                   .WithShipSatellite(SkTelinkSatellite(1L, "UNIQUE_SHIP_KEY", "SATELLITE_ID"))
                   .WithExternalShipId("SATELLITE_ID")
                   .WithIsUseKtsat(true)
                   .WithSkTelinkCompanyShip(SkTelinkCompanyShip(1L, "UNIQUE_SHIP_KEY", "UNIQUE_COMPANY_NAME"))
                   .Build();
            _mockShipInfoRepository
                .Setup(e => e.GetByShipKeyAsync(param.ShipKey))
                .ReturnsAsync(oldShipInfo);

            ShipInfo? capturedEntity = null;
            _mockShipInfoRepository.Setup(e => e.UpsertAsync(It.IsAny<ShipInfo>()))
                .Callback<ShipInfo>(arg => capturedEntity = arg)
                .ReturnsAsync((ShipInfo entity) => entity);

            // Act
            await _sut.Process(param);

            // Assert
            capturedEntity.Should().NotBeNull();

            capturedEntity.ShipServices.Should().ContainEquivalentOf(KtSatService(1L, "UNIQUE_SHIP_KEY"));

            capturedEntity.ShipSatellite.Should().BeEquivalentTo(KtSatellite(1L, "UNIQUE_SHIP_KEY", "SATELLITE_ID"),
                options => options.Excluding(s => s.UpdateDateTime));
            capturedEntity.ExternalShipId.Should().Be("SATELLITE_ID");
            capturedEntity.IsUseKtsat.Should().BeTrue();

            capturedEntity.SkTelinkCompanyShip.Should().BeNull();
        }

        [Fact(DisplayName = "SK TELINK의 CompanyName을 업데이트 한다")]
        public async Task Case17()
        {
            // Arrange
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

            var oldShipInfo = ShipInfo()
                   .WithId(1L)
                   .WithShipKey(param.ShipKey)
                   .WithCallsign(param.Callsign)
                   .WithShipName(param.ShipName)
                   .WithShipType(ShipTypes.Fishing)
                   .WithShipCode(param.ShipCode)
                   .WithShipServices(KtSatService(1L, "UNIQUE_SHIP_KEY"))
                   .WithShipSatellite(SkTelinkSatellite(1L, "UNIQUE_SHIP_KEY", "SATELLITE_ID"))
                   .WithExternalShipId("SATELLITE_ID")
                   .WithIsUseKtsat(true)
                   .WithSkTelinkCompanyShip(SkTelinkCompanyShip(1L, "UNIQUE_SHIP_KEY", "OLD_COMPANY_NAME"))
                   .Build();
            _mockShipInfoRepository
                .Setup(e => e.GetByShipKeyAsync(param.ShipKey))
                .ReturnsAsync(oldShipInfo);

            ShipInfo? capturedEntity = null;
            _mockShipInfoRepository.Setup(e => e.UpsertAsync(It.IsAny<ShipInfo>()))
                .Callback<ShipInfo>(arg => capturedEntity = arg)
                .ReturnsAsync((ShipInfo entity) => entity);

            // Act
            await _sut.Process(param);

            // Assert
            capturedEntity.Should().NotBeNull();
            capturedEntity.SkTelinkCompanyShip.Should().NotBeNull();
            capturedEntity.SkTelinkCompanyShip.CompanyName
                .Should().Be("UPDATE_COMPANY_NAME");
        }

        [Fact(DisplayName = "SK TELINK의 CompanyName 업데이트시 다른 엔티티는 변경되지 않는다")]
        public async Task Case18()
        {
            // Arrange
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

            var oldShipInfo = ShipInfo()
                   .WithId(1L)
                   .WithShipKey(param.ShipKey)
                   .WithCallsign(param.Callsign)
                   .WithShipName(param.ShipName)
                   .WithShipType(ShipTypes.Fishing)
                   .WithShipCode(param.ShipCode)
                   .WithShipServices(KtSatService(1L, "UNIQUE_SHIP_KEY"))
                   .WithShipSatellite(SkTelinkSatellite(1L, "UNIQUE_SHIP_KEY", "SATELLITE_ID"))
                   .WithExternalShipId("SATELLITE_ID")
                   .WithIsUseKtsat(true)
                   .WithSkTelinkCompanyShip(SkTelinkCompanyShip(1L, "UNIQUE_SHIP_KEY", "OLD_COMPANY_NAME"))
                   .Build();
            _mockShipInfoRepository
                .Setup(e => e.GetByShipKeyAsync(param.ShipKey))
                .ReturnsAsync(oldShipInfo);

            ShipInfo? capturedEntity = null;
            _mockShipInfoRepository.Setup(e => e.UpsertAsync(It.IsAny<ShipInfo>()))
                .Callback<ShipInfo>(arg => capturedEntity = arg)
                .ReturnsAsync((ShipInfo entity) => entity);

            // Act
            await _sut.Process(param);

            // Assert
            capturedEntity.Should().NotBeNull();
            capturedEntity.ShipServices.Should()
                .ContainEquivalentOf(KtSatService(1L, "UNIQUE_SHIP_KEY"));
            capturedEntity.ShipSatellite.Should()
                .BeEquivalentTo(SkTelinkSatellite(1L, "UNIQUE_SHIP_KEY", "SATELLITE_ID"));
        }

    }
}
