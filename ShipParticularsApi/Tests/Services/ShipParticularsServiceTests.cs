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
            capturedEntity.Id.Should().Be(1L);
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
            capturedEntity.Id.Should().Be(1L);
            capturedEntity.IsUseAis.Should().BeTrue();
            capturedEntity.ShipServices.Should().HaveCount(1);
            capturedEntity.ShipServices.Should().ContainEquivalentOf(SatAisService(1L, param.ShipKey));
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

            public ShipSatelliteParam? ShipSatelliteParam { get; set; }
            public SkTelinkCompanyShipParam? SkTelinkCompanyShipParam { get; set; }
        }

        public class ShipSatelliteParam
        {
            public string SatelliteType { get; set; }
            public string SatelliteId { get; set; }
        }

        public class SkTelinkCompanyShipParam
        {
            public string CompanyName { get; set; }
        }
    }
}
