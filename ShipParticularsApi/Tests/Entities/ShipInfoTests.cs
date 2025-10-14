using FluentAssertions;
using ShipParticularsApi.Entities;
using Xunit;
using static ShipParticularsApi.Tests.Builders.ShipInfoTestBuilder;
using static ShipParticularsApi.Tests.Builders.ShipServiceTestBuilder;
using static ShipParticularsApi.Tests.Services.ShipParticularsServiceTests;

namespace ShipParticularsApi.Tests.Entities
{
    public class ShipInfoTests
    {
        public class When_shipInfo_is_new
        {
            [Fact(DisplayName = "신규 ShipInfo이고, AIS 토글이 Off인 경우 ShipServices가 비어있다.")]
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

            [Fact(DisplayName = "신규 ShipInfo이고 AIS Toggle On인 경우 ShipServices가 하나 있다")]
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
        }

    }
}
