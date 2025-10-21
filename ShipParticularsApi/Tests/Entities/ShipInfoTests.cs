using FluentAssertions;
using ShipParticularsApi.Entities.Enums;
using ShipParticularsApi.ValueObjects;
using Xunit;
using static ShipParticularsApi.Tests.Builders.ShipInfoTestBuilder;
using static ShipParticularsApi.Tests.Builders.ShipSatelliteTestBuilder;
using static ShipParticularsApi.Tests.Builders.ShipServiceTestBuilder;
using static ShipParticularsApi.Tests.Builders.SkTelinkCompanyShipTestBuilder;

namespace ShipParticularsApi.Tests.Entities
{
    // TODO. Test Fixture 중복 제거
    public class ShipInfoTests
    {
        private const string FixedUserId = "TEST_USER_01";

        public class When_shipInfo_is_new
        {
            [Fact(DisplayName = "AIS 토글이 Off인 경우 ShipServices가 비어있다.")]
            public void Case1()
            {
                // Arrange
                const bool isAisToggleOn = false;
                var shipInfoDetails = new ShipInfoDetails(
                    "NEW_SHIP_KEY", "NEW_CALLSIGN", "NEW_SHIP_NAME", "FISHING", "NEW_SHIP_CODE"
                );

                var newShipInfo = ShipParticularsApi.Entities.ShipInfo.From(shipInfoDetails);

                // Act
                newShipInfo.ManageAisService(isAisToggleOn);

                // Assert
                var expected = ShipInfo().WithShipKey(shipInfoDetails.ShipKey)
                    .WithCallsign(shipInfoDetails.Callsign)
                    .WithShipName(shipInfoDetails.ShipName)
                    .WithShipType(ShipTypes.Fishing)
                    .WithShipCode(shipInfoDetails.ShipCode)
                    .WithIsUseAis(false)
                    .Build();

                newShipInfo.Should().NotBeNull();
                newShipInfo.Should().BeEquivalentTo(expected);
            }

            [Fact(DisplayName = "AIS Toggle On인 경우 ShipServices가 하나 있다")]
            public void Case2()
            {
                // Arrange
                const bool isAisToggleOn = true;
                var shipInfoDetails = new ShipInfoDetails(
                    "NEW_SHIP_KEY", "NEW_CALLSIGN", "NEW_SHIP_NAME", "FISHING", "NEW_SHIP_CODE"
                );
                var newShipInfo = ShipParticularsApi.Entities.ShipInfo.From(shipInfoDetails);

                // Act
                newShipInfo.ManageAisService(isAisToggleOn);

                // Assert
                var expected = ShipInfo().WithShipKey(shipInfoDetails.ShipKey)
                    .WithCallsign(shipInfoDetails.Callsign)
                    .WithShipName(shipInfoDetails.ShipName)
                    .WithShipType(ShipTypes.Fishing)
                    .WithShipCode(shipInfoDetails.ShipCode)
                    .WithIsUseAis(true)
                    .WithShipServices(SatAisService(0L, shipInfoDetails.ShipKey))
                    .Build();

                newShipInfo.Should().NotBeNull();
                newShipInfo.Should().BeEquivalentTo(expected);
            }

            [Fact(DisplayName = "GPS Toggle Off인 경우 ShipServices가 비어있다.")]
            public void Case3()
            {
                // Arrange
                const bool isGpsToggleOn = false;
                var satelliteDetails = new SatelliteDetails(null, null, null);

                var shipInfoDetails = new ShipInfoDetails(
                    "NEW_SHIP_KEY", "NEW_CALLSIGN", "NEW_SHIP_NAME", "FISHING", "NEW_SHIP_CODE"
                );
                var newShipInfo = ShipParticularsApi.Entities.ShipInfo.From(shipInfoDetails);

                // Act
                newShipInfo.ManageGpsService(isGpsToggleOn, satelliteDetails, null);

                // Assert
                var expected = ShipInfo().WithShipKey(shipInfoDetails.ShipKey)
                    .WithCallsign(shipInfoDetails.Callsign)
                    .WithShipName(shipInfoDetails.ShipName)
                    .WithShipType(ShipTypes.Fishing)
                    .WithShipCode(shipInfoDetails.ShipCode)
                    .Build();

                newShipInfo.Should().NotBeNull();
                newShipInfo.Should().BeEquivalentTo(expected);
            }

            [Fact(DisplayName = "GPS Toggle On & SatelliteType이 SK_TELINK가 아닌 경우 ShipService, ShipSatellite만 추가된다")]
            public void Case4()
            {
                // Arrange
                const bool isGpsToggleOn = true;
                var satelliteDetails = new SatelliteDetails("TEST_SATELLITE_ID", "KT_SAT", null);

                var shipInfoDetails = new ShipInfoDetails(
                    "NEW_SHIP_KEY", "NEW_CALLSIGN", "NEW_SHIP_NAME", "FISHING", "NEW_SHIP_CODE"
                );
                var newShipInfo = ShipParticularsApi.Entities.ShipInfo.From(shipInfoDetails);

                // Act
                newShipInfo.ManageGpsService(isGpsToggleOn, satelliteDetails, FixedUserId);

                // Assert
                var expected = ShipInfo().WithShipKey(shipInfoDetails.ShipKey)
                    .WithCallsign(shipInfoDetails.Callsign)
                    .WithShipName(shipInfoDetails.ShipName)
                    .WithShipType(ShipTypes.Fishing)
                    .WithShipCode(shipInfoDetails.ShipCode)
                    .WithShipServices(KtSatService(0L, shipInfoDetails.ShipKey))
                    .WithShipSatellite(ShipSatellite()
                        .WithShipKey(shipInfoDetails.ShipKey)
                        .WithSatelliteType(SatelliteTypes.KtSat)
                        .WithSatelliteId(satelliteDetails.SatelliteId)
                        .WithIsUseSatellite(true)
                        .WithCreateUserId(FixedUserId)
                        .Build())
                    .WithExternalShipId(satelliteDetails.SatelliteId)
                    .WithIsUseKtsat(true)
                    .Build();

                newShipInfo.Should().NotBeNull();
                newShipInfo.Should().BeEquivalentTo(expected,
                    options => options.Excluding(s => s.ShipSatellite.UpdateDateTime));
            }

            [Fact(DisplayName = "GPS Toggle On & SatelliteType이 SK_TELINK인 경우 ShipService, ShipSatellite, SkTelinkCompanyShip이 추가된다")]
            public void Case5()
            {
                // Arrange
                const bool isGpsToggleOn = true;
                var satelliteDetails = new SatelliteDetails("TEST_SATELLITE_ID", "SK_TELINK", "TEST_COMPANY_KEY");

                var shipInfoDetails = new ShipInfoDetails(
                    "NEW_SHIP_KEY", "NEW_CALLSIGN", "NEW_SHIP_NAME", "FISHING", "NEW_SHIP_CODE"
                );

                var newShipInfo = ShipParticularsApi.Entities.ShipInfo.From(shipInfoDetails);

                // Act
                newShipInfo.ManageGpsService(isGpsToggleOn, satelliteDetails, FixedUserId);

                // Assert
                var expected = ShipInfo().WithShipKey(shipInfoDetails.ShipKey)
                    .WithCallsign(shipInfoDetails.Callsign)
                    .WithShipName(shipInfoDetails.ShipName)
                    .WithShipType(ShipTypes.Fishing)
                    .WithShipCode(shipInfoDetails.ShipCode)
                    .WithShipServices(KtSatService(0L, shipInfoDetails.ShipKey))
                    .WithShipSatellite(
                        ShipSatellite()
                        .WithShipKey(shipInfoDetails.ShipKey)
                        .WithSatelliteType(SatelliteTypes.SkTelink)
                        .WithSatelliteId(satelliteDetails.SatelliteId)
                        .WithIsUseSatellite(true)
                        .WithCreateUserId(FixedUserId)
                        .Build())
                    .WithSkTelinkCompanyShip(SkTelinkCompanyShip(shipInfoDetails.ShipKey, satelliteDetails.CompanyName))
                    .WithExternalShipId(satelliteDetails.SatelliteId)
                    .WithIsUseKtsat(true)
                    .Build();

                newShipInfo.Should().NotBeNull();
                newShipInfo.Should().BeEquivalentTo(expected,
                    options => options.Excluding(s => s.ShipSatellite.UpdateDateTime));
            }
        }
        public class When_shipInfo_already_exist
        {
            [Fact(DisplayName = "기존 ShipInfo 정보를 업데이트한다")]
            public void Case6()
            {
                // Arrange
                var shipInfoDetails = new ShipInfoDetails(
                    "UNIQUE_SHIP_KEY",
                    "UPDATE_CALLSIGN",
                    "UPDATE_SHIP_NAME",
                    "PASSENGER",
                    "UPDATE_SHIP_CODE"
                );

                var target = ShipInfo()
                    .WithId(1L)
                    .WithShipKey("UNIQUE_SHIP_KEY")
                    .WithCallsign("OLD_CALLSIGN")
                    .WithShipName("OLD_SHIP_NAME")
                    .WithShipType(ShipTypes.Fishing)
                    .WithShipCode("OLD_SHIP_CODE")
                    .Build();

                // Act
                target = target.UpdateDetails(shipInfoDetails);

                // Assert
                target.Should().BeEquivalentTo(
                    ShipInfo().WithId(1L)
                        .WithShipKey("UNIQUE_SHIP_KEY")
                        .WithCallsign("UPDATE_CALLSIGN")
                        .WithShipName("UPDATE_SHIP_NAME")
                        .WithShipType(ShipTypes.Passenger)
                        .WithShipCode("UPDATE_SHIP_CODE")
                        .Build()
                );
            }

            [Fact(DisplayName = "AIS Toggle On인 경우 서비스('sat-ais')가 추가된다")]
            public void Case7()
            {
                // Arrange
                const bool isAisToggleOn = true;
                var target = ShipInfo()
                    .WithId(1L)
                    .WithShipKey("UNIQUE_SHIP_KEY")
                    .Build();

                // Act
                target.ManageAisService(isAisToggleOn);

                // Assert
                target.ShipServices.Should().NotBeEmpty();
                target.ShipServices.Should().ContainEquivalentOf(SatAisService(target.ShipKey));
                target.IsUseAis.Should().BeTrue();
            }

            [Fact(DisplayName = "'sat-ais' ShipService 사용 중일때, AIS Toggle On해도 아무것도 하지 않는다")]
            public void Case8()
            {
                // Arrange
                const bool isAisToggleOn = true;
                var target = ShipInfo()
                    .WithId(1L)
                    .WithShipKey("UNIQUE_SHIP_KEY")
                    .WithShipServices(SatAisService(1L, "UNIQUE_SHIP_KEY"))
                    .WithIsUseAis(true)
                    .Build();

                // Act
                target.ManageAisService(isAisToggleOn);

                // Assert
                target.ShipServices.Should().HaveCount(1);
                target.ShipServices.Should().ContainEquivalentOf(SatAisService(1L, target.ShipKey));
                target.IsUseAis.Should().BeTrue();
            }

            [Fact(DisplayName = "'sat-ais' ShipService를 사용 중이고, AIS Toggle Off하는 경우 컬렉션 길이가 0이 된다")]
            public void Case9()
            {
                // Arrange
                const bool isAisToggleOn = false;
                var target = ShipInfo()
                    .WithId(1L)
                    .WithShipKey("UNIQUE_SHIP_KEY")
                    .WithShipServices(SatAisService(1L, "UNIQUE_SHIP_KEY"))
                    .Build();

                // Act
                target.ManageAisService(isAisToggleOn);

                // Assert 
                target.ShipServices.Should().BeEmpty();
                target.IsUseAis.Should().BeFalse();
            }

            // TODO. Case6가 동일해보임
            [Fact(DisplayName = "등록된 ShipService가 없는 경우, AIS Toggle Off해도 아무것도 하지 않는다")]
            public void Case10()
            {
                // Arrange
                const bool isAisToggleOn = false;
                var target = ShipInfo()
                    .WithId(1L)
                    .WithShipKey("UNIQUE_SHIP_KEY")
                    .Build();

                // Act
                target.ManageAisService(isAisToggleOn);

                // Assert 
                target.ShipServices.Should().BeEmpty();
                target.IsUseAis.Should().BeFalse();
            }

            [Fact(DisplayName = "등록된 ShipService가 없는 경우, GPS Toggle Off 해도 아무것도 하지 않는다")]
            public void Case11()
            {
                // Arrange
                const bool isGPSToggleOn = false;
                var satelliteDetails = new SatelliteDetails(null, null, null);

                var target = ShipInfo()
                    .WithId(1L)
                    .WithShipKey("UNIQUE_SHIP_KEY")
                    .Build();

                // Act
                target.ManageGpsService(isGPSToggleOn, satelliteDetails, FixedUserId);

                // Assert
                target.ShipServices.Should().BeEmpty();

                target.ShipSatellite.Should().BeNull();
                target.IsUseKtsat.Should().BeNull();
                target.ExternalShipId.Should().BeNull();

                target.SkTelinkCompanyShip.Should().BeNull();
            }

            [Fact(DisplayName = "사용중인 GPS 서비스를 비활성화하면 관련 엔티티와 필드를 초기화한다")]
            public void Case12()
            {
                // Arrange
                const bool isGPSToggleOn = false;
                var satelliteDetails = new SatelliteDetails(null, null, null);

                var target = ShipInfo()
                    .WithId(1L)
                    .WithShipKey("UNIQUE_SHIP_KEY")
                    .WithShipServices(KtSatService(1L, "UNIQUE_SHIP_KEY"))
                    .WithShipSatellite(SkTelinkSatellite(1L, "UNIQUE_SHIP_KEY", "SATELLITE_ID"))
                    .WithExternalShipId("SATELLITE_ID")
                    .WithIsUseKtsat(true)
                    .WithSkTelinkCompanyShip(SkTelinkCompanyShip(1L, "UNIQUE_SHIP_KEY", "UNIQUE_COMPANY_NAME"))
                    .Build();

                // Act
                // NOTE. ShipService는 고정('kt-sat'), ShipSatellite, SkTelinkCompanyShip
                target.ManageGpsService(isGPSToggleOn, satelliteDetails, FixedUserId);

                // Assert
                target.ShipServices.Should().BeEmpty();

                target.ShipSatellite.Should().BeNull();
                target.IsUseKtsat.Should().BeFalse();
                target.ExternalShipId.Should().BeNull();

                target.SkTelinkCompanyShip.Should().BeNull();
            }

            [Fact(DisplayName = "KT_SAT 서비스를 신규 사용하는 경우, ShipService와 ShipSatellite가 등록된다")]
            public void Case13()
            {
                // Arrange
                const bool isGpsToggleOn = true;
                var satelliteDetails = new SatelliteDetails("SATELLITE_ID", "KT_SAT", null);

                var target = ShipInfo()
                    .WithId(1L)
                    .WithShipKey("UNIQUE_SHIP_KEY")
                    .Build();

                // Act
                target.ManageGpsService(isGpsToggleOn, satelliteDetails, FixedUserId);

                // Assert
                target.ShipServices.Should().ContainEquivalentOf(KtSatService("UNIQUE_SHIP_KEY"));

                target.ShipSatellite.Should().BeEquivalentTo(
                     ShipSatellite()
                        .WithShipKey("UNIQUE_SHIP_KEY")
                        .WithSatelliteType(SatelliteTypes.KtSat)
                        .WithSatelliteId("SATELLITE_ID")
                        .WithIsUseSatellite(true)
                        .WithCreateUserId(FixedUserId)
                        .Build(),
                    options => options.Excluding(s => s.UpdateDateTime));
                target.ExternalShipId.Should().Be("SATELLITE_ID");
                target.IsUseKtsat.Should().BeTrue();

                target.SkTelinkCompanyShip.Should().BeNull();
            }

            [Fact(DisplayName = "신규로 SK_TELINK 서비스 사용하는 경우, ShipService,ShipSatellite, SkTelinkCompanyShip가 등록된다")]
            public void Case14()
            {
                // Arrange
                const bool isGpsToggleOn = true;
                var satelliteDetails = new SatelliteDetails("SATELLITE_ID", "SK_TELINK", "UNIQUE_COMPANY_NAME");

                var target = ShipInfo()
                    .WithId(1L)
                    .WithShipKey("UNIQUE_SHIP_KEY")
                    .Build();

                // Act
                target.ManageGpsService(isGpsToggleOn, satelliteDetails, FixedUserId);

                // Assert
                target.ShipServices.Should().ContainEquivalentOf(KtSatService("UNIQUE_SHIP_KEY"));
                target.ShipSatellite.Should().BeEquivalentTo(
                    ShipSatellite()
                        .WithShipKey("UNIQUE_SHIP_KEY")
                        .WithSatelliteType(SatelliteTypes.SkTelink)
                        .WithSatelliteId("SATELLITE_ID")
                        .WithIsUseSatellite(true)
                        .WithCreateUserId(FixedUserId)
                        .Build(),
                    options => options.Excluding(s => s.UpdateDateTime));
                target.SkTelinkCompanyShip.Should().BeEquivalentTo(SkTelinkCompanyShip("UNIQUE_SHIP_KEY", "UNIQUE_COMPANY_NAME"));

                target.ExternalShipId.Should().Be("SATELLITE_ID");
                target.IsUseKtsat.Should().BeTrue();
            }

            [Fact(DisplayName = "KT_SAT에서 SK_TELINK로 서비스 변경하는 경우, ShipSatellite가 업데이트되고, SKTelinkCompanyShip이 추가된다")]
            public void Case15()
            {
                const bool isGpsToggleOn = true;
                var satelliteDetails = new SatelliteDetails("SATELLITE_ID", "SK_TELINK", "UNIQUE_COMPANY_NAME");

                var target = ShipInfo()
                    .WithId(1L)
                    .WithShipKey("UNIQUE_SHIP_KEY")
                    .WithShipServices(KtSatService(1L, "UNIQUE_SHIP_KEY"))
                    .WithShipSatellite(ShipSatellite()
                        .WithId(1L)
                        .WithShipKey("UNIQUE_SHIP_KEY")
                        .WithSatelliteType(SatelliteTypes.KtSat)
                        .WithSatelliteId("SATELLITE_ID")
                        .WithIsUseSatellite(true)
                        .WithCreateUserId(FixedUserId)
                        .Build())
                    .WithExternalShipId("SATELLITE_ID")
                    .WithIsUseKtsat(true)
                    .Build();

                // Act
                target.ManageGpsService(isGpsToggleOn, satelliteDetails, FixedUserId);

                // Assert
                target.ShipServices.Should().ContainEquivalentOf(KtSatService(1L, "UNIQUE_SHIP_KEY"));
                target.ShipSatellite.Should().BeEquivalentTo(
                    ShipSatellite()
                        .WithId(1L)
                        .WithShipKey("UNIQUE_SHIP_KEY")
                        .WithSatelliteType(SatelliteTypes.SkTelink)
                        .WithSatelliteId("SATELLITE_ID")
                        .WithIsUseSatellite(true)
                        .WithCreateUserId(FixedUserId)
                        .WithUpdateUserId(FixedUserId)
                        .Build()
                );
                target.SkTelinkCompanyShip.Should().BeEquivalentTo(SkTelinkCompanyShip("UNIQUE_SHIP_KEY", "UNIQUE_COMPANY_NAME"));

                target.ExternalShipId.Should().Be("SATELLITE_ID");
                target.IsUseKtsat.Should().BeTrue();
            }

            [Fact(DisplayName = "SK_TELINK에서 KT_SAT로 서비스 변경하는 경우, ShipSatellite가 업데이트되고, SKTelinkCompanyShip이 제거된다")]
            public void Case16()
            {
                const bool isGpsToggleOn = true;
                var satelliteDetails = new SatelliteDetails("SATELLITE_ID", "KT_SAT", null);

                var target = ShipInfo()
                    .WithId(1L)
                    .WithShipKey("UNIQUE_SHIP_KEY")
                    .WithShipServices(KtSatService(1L, "UNIQUE_SHIP_KEY"))
                    .WithShipSatellite(
                        ShipSatellite()
                        .WithId(1L)
                        .WithShipKey("UNIQUE_SHIP_KEY")
                        .WithSatelliteType(SatelliteTypes.SkTelink)
                        .WithSatelliteId("SATELLITE_ID")
                        .WithIsUseSatellite(true)
                        .WithCreateUserId(FixedUserId)
                        .Build()
                    )
                    .WithExternalShipId("SATELLITE_ID")
                    .WithIsUseKtsat(true)
                    .WithSkTelinkCompanyShip(SkTelinkCompanyShip(1L, "UNIQUE_SHIP_KEY", "UNIQUE_COMPANY_NAME"))
                    .Build();

                // Act
                target.ManageGpsService(isGpsToggleOn, satelliteDetails, FixedUserId);

                // Assert
                target.ShipServices.Should().ContainEquivalentOf(KtSatService(1L, "UNIQUE_SHIP_KEY"));
                target.ShipSatellite.Should().BeEquivalentTo(
                    ShipSatellite()
                        .WithId(1L)
                        .WithShipKey("UNIQUE_SHIP_KEY")
                        .WithSatelliteType(SatelliteTypes.KtSat)
                        .WithSatelliteId("SATELLITE_ID")
                        .WithIsUseSatellite(true)
                        .WithCreateUserId(FixedUserId)
                        .WithUpdateUserId(FixedUserId)
                        .Build(),
                    options => options.Excluding(s => s.UpdateDateTime));
                target.SkTelinkCompanyShip.Should().BeNull();

                target.ExternalShipId.Should().Be("SATELLITE_ID");
                target.IsUseKtsat.Should().BeTrue();
            }


            [Fact(DisplayName = "SK TELINK의 CompanyName을 업데이트 한다")]
            public void Case17()
            {
                const bool isGpsToggleOn = true;
                var satelliteDetails = new SatelliteDetails("SATELLITE_ID", "SK_TELINK", "UPDATE_COMPANY_NAME");

                var target = ShipInfo()
                    .WithId(1L)
                    .WithShipKey("UNIQUE_SHIP_KEY")
                    .WithShipServices(KtSatService(1L, "UNIQUE_SHIP_KEY"))
                    .WithShipSatellite(SkTelinkSatellite(1L, "UNIQUE_SHIP_KEY", "SATELLITE_ID"))
                    .WithExternalShipId("SATELLITE_ID")
                    .WithIsUseKtsat(true)
                    .WithSkTelinkCompanyShip(SkTelinkCompanyShip(1L, "UNIQUE_SHIP_KEY", "UNIQUE_COMPANY_NAME"))
                    .Build();

                // Act
                target.ManageGpsService(isGpsToggleOn, satelliteDetails, FixedUserId);

                // Assert
                target.SkTelinkCompanyShip.Should()
                    .BeEquivalentTo(SkTelinkCompanyShip(1L, "UNIQUE_SHIP_KEY", "UPDATE_COMPANY_NAME"));
            }

            [Fact(DisplayName = "SK TELINK의 CompanyName 업데이트시 다른 엔티티는 변경되지 않는다")]
            public void Case18()
            {
                const bool isGpsToggleOn = true;
                var satelliteDetails = new SatelliteDetails("SATELLITE_ID", "SK_TELINK", "UPDATE_COMPANY_NAME");

                var shipService = KtSatService(1L, "UNIQUE_SHIP_KEY");
                var skTelinkSatellite = SkTelinkSatellite(1L, "UNIQUE_SHIP_KEY", "SATELLITE_ID");
                var target = ShipInfo()
                    .WithId(1L)
                    .WithShipKey("UNIQUE_SHIP_KEY")
                    .WithShipServices(shipService)
                    .WithShipSatellite(skTelinkSatellite)
                    .WithExternalShipId("SATELLITE_ID")
                    .WithIsUseKtsat(true)
                    .WithSkTelinkCompanyShip(SkTelinkCompanyShip(1L, "UNIQUE_SHIP_KEY", "UNIQUE_COMPANY_NAME"))
                    .Build();

                // Act
                target.ManageGpsService(isGpsToggleOn, satelliteDetails, FixedUserId);

                // Assert
                target.ShipServices.Should().ContainEquivalentOf(shipService);
                target.ShipSatellite.Should().BeEquivalentTo(skTelinkSatellite);
            }
        }

    }
}
