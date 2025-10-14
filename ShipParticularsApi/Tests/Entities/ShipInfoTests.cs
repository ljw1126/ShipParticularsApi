using FluentAssertions;
using ShipParticularsApi.Entities;
using Xunit;
using static ShipParticularsApi.Tests.Builders.ShipInfoTestBuilder;
using static ShipParticularsApi.Tests.Builders.ShipSatelliteTestBuilder;
using static ShipParticularsApi.Tests.Builders.ShipServiceTestBuilder;
using static ShipParticularsApi.Tests.Builders.SkTelinkCompanyShipTestBuilder;
using static ShipParticularsApi.Tests.Services.ShipParticularsServiceTests;

namespace ShipParticularsApi.Tests.Entities
{
    public class ShipInfoTests
    {
        public class When_shipInfo_is_new
        {
            [Fact(DisplayName = "AIS 토글이 Off인 경우 ShipServices가 비어있다.")]
            public void Case1()
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
                var newShipInfo = ShipParticularsApi.Entities.ShipInfo.From(param);

                // Act
                newShipInfo.ManageAisService(param.IsAisToggleOn);

                // Assert
                var expected = ShipInfo().WithShipKey(param.ShipKey)
                    .WithCallsign(param.Callsign)
                    .WithShipName(param.ShipName)
                    .WithShipType(ShipTypes.Fishing)
                    .WithShipCode(param.ShipCode)
                    .WithIsUseAis(false)
                    .Build();

                newShipInfo.Should().NotBeNull();
                newShipInfo.Should().BeEquivalentTo(expected);
            }

            [Fact(DisplayName = "AIS Toggle On인 경우 ShipServices가 하나 있다")]
            public void Case2()
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
                var newShipInfo = ShipParticularsApi.Entities.ShipInfo.From(param);

                // Act
                newShipInfo.ManageAisService(true);

                // Assert
                var expected = ShipInfo().WithShipKey(param.ShipKey)
                    .WithCallsign(param.Callsign)
                    .WithShipName(param.ShipName)
                    .WithShipType(ShipTypes.Fishing)
                    .WithShipCode(param.ShipCode)
                    .WithIsUseAis(true)
                    .WithShipServices(SatAisService(0L, param.ShipKey))
                    .Build();

                newShipInfo.Should().NotBeNull();
                newShipInfo.Should().BeEquivalentTo(expected);
            }

            [Fact(DisplayName = "GPS Toggle Off인 경우 ShipServices가 비어있다.")]
            public void Case3()
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
                var newShipInfo = ShipParticularsApi.Entities.ShipInfo.From(param);

                // Act
                newShipInfo.ManageGpsService(
                    param.IsGPSToggleOn,
                    null,
                    null,
                    null);

                // Assert
                var expected = ShipInfo().WithShipKey(param.ShipKey)
                    .WithCallsign(param.Callsign)
                    .WithShipName(param.ShipName)
                    .WithShipType(ShipTypes.Fishing)
                    .WithShipCode(param.ShipCode)
                    .Build();

                newShipInfo.Should().NotBeNull();
                newShipInfo.Should().BeEquivalentTo(expected);
            }

            [Fact(DisplayName = "GPS Toggle On & SatelliteType이 SK가 아닌 경우 ShipService, ShipSatellite만 추가된다")]
            public void Case4()
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
                var newShipInfo = ShipParticularsApi.Entities.ShipInfo.From(param);

                // Act
                newShipInfo.ManageGpsService(
                    param.IsGPSToggleOn,
                    param.ShipSatelliteParam.SatelliteId,
                    param.ShipSatelliteParam.SatelliteType,
                    null);

                // Assert
                var expected = ShipInfo().WithShipKey(param.ShipKey)
                    .WithCallsign(param.Callsign)
                    .WithShipName(param.ShipName)
                    .WithShipType(ShipTypes.Fishing)
                    .WithShipCode(param.ShipCode)
                    .WithShipServices(KtSatService(0L, param.ShipKey))
                    .WithShipSatellite(KtSatellite(param.ShipKey, param.ShipSatelliteParam.SatelliteId))
                    .Build();

                newShipInfo.Should().NotBeNull();
                newShipInfo.Should().BeEquivalentTo(expected,
                    options => options.Excluding(s => s.ShipSatellite.UpdateDateTime));
            }

            [Fact(DisplayName = "GPS Toggle On & SatelliteType이 SK인 경우 ShipService, ShipSatellite, SkTelinkCompanyShip이 추가된다")]
            public void Case5()
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
                var newShipInfo = ShipParticularsApi.Entities.ShipInfo.From(param);

                // Act
                newShipInfo.ManageGpsService(
                   param.IsGPSToggleOn,
                   param.ShipSatelliteParam.SatelliteId,
                   param.ShipSatelliteParam.SatelliteType,
                   param.SkTelinkCompanyShipParam.CompanyName);

                // Assert
                var expected = ShipInfo().WithShipKey(param.ShipKey)
                    .WithCallsign(param.Callsign)
                    .WithShipName(param.ShipName)
                    .WithShipType(ShipTypes.Fishing)
                    .WithShipCode(param.ShipCode)
                    .WithShipServices(KtSatService(0L, param.ShipKey))
                    .WithShipSatellite(SkTelinkSatellite(param.ShipKey, param.ShipSatelliteParam.SatelliteId))
                    .WithSkTelinkCompanyShip(SkTelinkCompanyShip(param.ShipKey, param.SkTelinkCompanyShipParam.CompanyName))
                    .Build();

                newShipInfo.Should().NotBeNull();
                newShipInfo.Should().BeEquivalentTo(expected,
                    options => options.Excluding(s => s.ShipSatellite.UpdateDateTime));
            }
        }

    }
}
