using FluentAssertions;
using Moq;
using ShipParticularsApi.Entities;
using ShipParticularsApi.Services;
using Xunit;
using static ShipParticularsApi.Tests.Builders.ShipSatelliteTestBuilder;
using static ShipParticularsApi.Tests.Builders.ShipServiceTestBuilder;
using static ShipParticularsApi.Tests.Builders.SkTelinkCompanyShipTestBuilder;
namespace ShipParticularsApi.Tests.Services
{
    public class ShipParticularsServiceTests
    {
        private readonly ShipParticularsService _sut;
        private readonly Mock<IShipInfoRepository> _mockShipInfoRepository;
        private ShipInfo? _capturedEntity;

        public ShipParticularsServiceTests()
        {
            _mockShipInfoRepository = new Mock<IShipInfoRepository>();
            _sut = new ShipParticularsService(_mockShipInfoRepository.Object);

            _capturedEntity = null;
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

            SetupShipInfoUpsertAsyncMock();

            // Act
            await _sut.Process(param);

            // Assert
            _mockShipInfoRepository.Verify(e => e.UpsertAsync(It.IsAny<ShipInfo>()), Times.Once);

            _capturedEntity.Should().NotBeNull();
            _capturedEntity.Id.Should().Be(1L);
            _capturedEntity.IsUseAis.Should().BeFalse();
            _capturedEntity.ShipServices.Should().BeEmpty();
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

            SetupShipInfoUpsertAsyncMock();

            // Act
            await _sut.Process(param);

            // Assert
            _mockShipInfoRepository.Verify(e => e.UpsertAsync(It.IsAny<ShipInfo>()), Times.Once);

            _capturedEntity.Should().NotBeNull();
            _capturedEntity.Id.Should().Be(1L);
            _capturedEntity.IsUseAis.Should().BeTrue();
            _capturedEntity.ShipServices.Should().HaveCount(1);
            _capturedEntity.ShipServices.Should().ContainEquivalentOf(SatAisService(1L, param.ShipKey));
        }

        [Fact(DisplayName = "GPS Toggle Off인 경우 ShipServices가 비어있다.")]
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

            SetupShipInfoUpsertAsyncMock();

            // Act
            await _sut.Process(param);

            // Assert
            _mockShipInfoRepository.Verify(e => e.UpsertAsync(It.IsAny<ShipInfo>()), Times.Once);

            _capturedEntity.Should().NotBeNull();
            _capturedEntity.Id.Should().Be(1L);

            _capturedEntity.ShipServices.Should().BeEmpty();
            _capturedEntity.ShipSatellite.Should().BeNull();
            _capturedEntity.SkTelinkCompanyShip.Should().BeNull();
        }

        [Fact(DisplayName = "GPS Toggle On & SatelliteType이 SK가 아닌 경우 ShipService, ShipSatellite만 추가된다")]
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

            SetupShipInfoUpsertAsyncMock();

            // Act
            await _sut.Process(param);

            // Assert
            _mockShipInfoRepository.Verify(e => e.UpsertAsync(It.IsAny<ShipInfo>()), Times.Once);

            _capturedEntity.Should().NotBeNull();
            _capturedEntity.Id.Should().Be(1L);

            _capturedEntity.ShipServices.Should().HaveCount(1)
                .And.ContainEquivalentOf(KtSatService(1L, param.ShipKey));

            _capturedEntity.ShipSatellite.Should().NotBeNull()
                .And.BeEquivalentTo(KtSatellite(1L, param.ShipKey, param.ShipSatelliteParam.SatelliteId),
                options => options.Excluding(s => s.UpdateDateTime));

            _capturedEntity.SkTelinkCompanyShip.Should().BeNull();
        }

        [Fact(DisplayName = "GPS Toggle On & SatelliteType이 SK인 경우 ShipService, ShipSatellite, SkTelinkCompanyShip이 추가된다")]
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

            SetupShipInfoUpsertAsyncMock();

            // Act
            await _sut.Process(param);

            // Assert
            _mockShipInfoRepository.Verify(e => e.UpsertAsync(It.IsAny<ShipInfo>()), Times.Once);

            _capturedEntity.Should().NotBeNull();
            _capturedEntity.Id.Should().Be(1L);

            _capturedEntity.ShipServices.Should().HaveCount(1)
                .And.ContainEquivalentOf(KtSatService(1L, param.ShipKey));

            _capturedEntity.ShipSatellite.Should().NotBeNull()
                .And.BeEquivalentTo(SkTelinkSatellite(1L, param.ShipKey, param.ShipSatelliteParam.SatelliteId),
                options => options.Excluding(s => s.UpdateDateTime));

            _capturedEntity.SkTelinkCompanyShip.Should().NotBeNull()
                .And.BeEquivalentTo(SkTelinkCompanyShip(1L, param.ShipKey, param.SkTelinkCompanyShipParam.CompanyName));
        }

        private void SetupShipInfoUpsertAsyncMock()
        {
            _mockShipInfoRepository
               .Setup(e => e.UpsertAsync(It.IsAny<ShipInfo>()))
               .ReturnsAsync((ShipInfo entity) =>
               {
                   entity.Id = 1L;
                   var ShipService = entity.ShipServices.FirstOrDefault();
                   if (ShipService != null)
                   {
                       ShipService.Id = 1L;
                   }

                   if (entity.ShipSatellite != null)
                   {
                       entity.ShipSatellite.Id = 1L;
                   }

                   if (entity.SkTelinkCompanyShip != null)
                   {
                       entity.SkTelinkCompanyShip.Id = 1L;
                   }

                   _capturedEntity = entity;
                   return entity;
               });
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
