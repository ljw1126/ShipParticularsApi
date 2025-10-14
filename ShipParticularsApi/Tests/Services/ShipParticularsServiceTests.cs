using FluentAssertions;
using Moq;
using ShipParticularsApi.Entities;
using ShipParticularsApi.Services;
using Xunit;

using static ShipParticularsApi.Tests.Builders.ShipServiceTestBuilder;

namespace ShipParticularsApi.Tests.Services
{
    public class ShipParticularsServiceTests
    {
        private readonly ShipParticularsService _sut;
        private readonly Mock<IReplaceShipNameRepository> _mockReplaceShipNameRepository;
        private readonly Mock<IShipInfoRepository> _mockShipInfoRepository;
        private readonly Mock<IShipModelTestRepository> _mockShipModelTestRepository;
        private readonly Mock<IShipSatelliteRepository> _mockShipSatelliteRepository;
        private readonly Mock<IShipServiceRepository> _mockShipServiceRepository;
        private readonly Mock<ISkTelinkCompanyShipRepository> _mockSkTelinkCompanyShipRepository;

        public ShipParticularsServiceTests()
        {
            _mockReplaceShipNameRepository = new Mock<IReplaceShipNameRepository>();
            _mockShipInfoRepository = new Mock<IShipInfoRepository>();
            _mockShipModelTestRepository = new Mock<IShipModelTestRepository>();
            _mockShipSatelliteRepository = new Mock<IShipSatelliteRepository>();
            _mockShipServiceRepository = new Mock<IShipServiceRepository>();
            _mockSkTelinkCompanyShipRepository = new Mock<ISkTelinkCompanyShipRepository>();

            _sut = new ShipParticularsService(
                _mockReplaceShipNameRepository.Object,
                _mockShipInfoRepository.Object,
                _mockShipModelTestRepository.Object,
                _mockShipSatelliteRepository.Object,
                _mockShipServiceRepository.Object,
                _mockSkTelinkCompanyShipRepository.Object
            );
        }

        // AIS Toggle Off, GPS Toggle Off, 신규 SHIP_INFO인 경우
        [Fact]
        public async Task Insert_new_shipInfo()
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

            _mockShipInfoRepository
                .Setup(e => e.UpsertAsync(It.IsAny<ShipInfo>()))
                .ReturnsAsync((ShipInfo entity) =>
                {
                    entity.Id = 1L;
                    return entity;
                });

            // Act
            await _sut.Process(param);

            // Assert
            _mockShipServiceRepository.Verify(e => e.GetByShipKeyAndServiceNameAsync(
                    It.IsAny<string>(),
                    It.IsAny<ServiceNameTypes>()
                ), Times.Once);
            _mockShipInfoRepository.Verify(e => e.UpsertAsync(It.Is<ShipInfo>(
                    s => s.ShipType == ShipTypes.Fishing && s.ShipKey == param.ShipKey
                )), Times.Once);
        }

        // AIS Toggle = false 인 경우에도 SHIP_SERVICE 조회 필요.
        // AIS Toggle 관련 SERVICE_NAME = "sat-ais"

        // GPS Toggle = false 인 경우에도 SHIP_SERVICE 조회 필요
        // GPS Toggle 관련 SERVICE_NAME = "kt-sat"

        [Fact]
        public async Task Insert_new_shipInfo_with_ais_toggle_off()
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

            _mockShipServiceRepository
               .Setup(e => e.GetByShipKeyAndServiceNameAsync(param.ShipKey, ServiceNameTypes.SatAis))
               .ReturnsAsync((ShipService?)null);

            _mockShipInfoRepository
                .Setup(e => e.UpsertAsync(It.IsAny<ShipInfo>()))
                .ReturnsAsync((ShipInfo entity) =>
                {
                    entity.Id = 1L;
                    return entity;
                });

            // Act
            await _sut.Process(param);

            // Assert
            _mockShipServiceRepository.Verify(e => e.GetByShipKeyAndServiceNameAsync(
                    It.IsAny<string>(),
                    It.IsAny<ServiceNameTypes>()
                ), Times.Once);
            _mockShipInfoRepository.Verify(e => e.UpsertAsync(It.Is<ShipInfo>(
                    s => s.ShipType == ShipTypes.Fishing && s.ShipKey == param.ShipKey
                )), Times.Once);
        }

        // 신규 ShipInfo이고, ShipService가 존재하지 않는 경우
        [Fact]
        public async Task Case2_1()
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

            _mockShipServiceRepository
                .Setup(e => e.GetByShipKeyAndServiceNameAsync(param.ShipKey, ServiceNameTypes.SatAis))
                .ReturnsAsync((ShipService?)null);

            ShipInfo? capturedEntity = null;
            _mockShipInfoRepository
                .Setup(e => e.UpsertAsync(It.IsAny<ShipInfo>()))
                .ReturnsAsync((ShipInfo entity) =>
                {
                    entity.Id = 1L;
                    var aisService = entity.ShipServices.FirstOrDefault();
                    if (aisService != null)
                    {
                        aisService.Id = 1L;
                    }

                    capturedEntity = entity;
                    return entity;
                });

            // Act
            await _sut.Process(param);

            // Assert
            _mockShipInfoRepository.Verify(e => e.UpsertAsync(It.IsAny<ShipInfo>()), Times.Once);

            capturedEntity.Should().NotBeNull();
            capturedEntity.IsUseAis.Should().BeTrue();

            capturedEntity.ShipServices.Should().HaveCount(1);
            capturedEntity.ShipServices.Should().ContainSingle()
                .Which.Should().Match<ShipService>(s =>
                    s.Id == 1L &&
                    s.ServiceName == ServiceNameTypes.SatAis
            );
        }

        // 신규 ShipInfo, AIS Toggle On 인데, ShipService(sat-ais)가 이미 존재하는 경우 (모순)
        [Fact]
        public async Task Case2_2()
        {// Arrange
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

            _mockShipServiceRepository
                .Setup(e => e.GetByShipKeyAndServiceNameAsync(param.ShipKey, ServiceNameTypes.SatAis))
                .ReturnsAsync(SatAisService(param.ShipKey));

            ShipInfo? capturedEntity = null;
            _mockShipInfoRepository
                .Setup(e => e.UpsertAsync(It.IsAny<ShipInfo>()))
                .ReturnsAsync((ShipInfo entity) =>
                {
                    entity.Id = 1L;
                    capturedEntity = entity;
                    return entity;
                });

            // Act
            await _sut.Process(param);

            // Assert
            _mockShipInfoRepository.Verify(e => e.UpsertAsync(It.IsAny<ShipInfo>()), Times.Once);

            capturedEntity.Should().NotBeNull();
            capturedEntity.IsUseAis.Should().BeTrue();

            capturedEntity.ShipServices.Should().HaveCount(1);
            capturedEntity.ShipServices.Should().ContainSingle()
                .Which.Should().Match<ShipService>(s =>
                    s.Id == 1L &&
                    s.ServiceName == ServiceNameTypes.SatAis
            );
        }

        // 신규 ShipInfo이고, AIS Toggle = false인데, ShipService가 존재하는 경우
        [Fact]
        public async Task Case2_3()
        {// Arrange
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

            _mockShipServiceRepository
                .Setup(e => e.GetByShipKeyAndServiceNameAsync(param.ShipKey, ServiceNameTypes.SatAis))
                .ReturnsAsync(SatAisService(param.ShipKey));

            ShipInfo? capturedEntity = null;
            _mockShipInfoRepository
                .Setup(e => e.UpsertAsync(It.IsAny<ShipInfo>()))
                .ReturnsAsync((ShipInfo entity) =>
                {
                    entity.Id = 1L;
                    capturedEntity = entity;
                    return entity;
                });

            // Act
            await _sut.Process(param);

            // Assert
            _mockShipInfoRepository.Verify(e => e.UpsertAsync(It.IsAny<ShipInfo>()), Times.Once);

            capturedEntity.Should().NotBeNull();
            capturedEntity.IsUseAis.Should().BeFalse();
            capturedEntity.ShipServices.Should().BeEmpty();
        }

        public class ShipParticularsParam
        {
            public bool IsAisToggleOn { get; set; } = false;
            public bool IsGPSToggleOn { get; set; } = false;

            public string ShipKey { get; set; }
            public string Callsign { get; set; }
            public string ShipName { get; set; }
            public string ShipType { get; set; }
            public string ShipCode { get; set; }
        }
    }
}
