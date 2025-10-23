using FluentAssertions;
using ShipParticularsApi.Entities.Enums;
using ShipParticularsApi.ValueObjects;
using Xunit;
using static ShipParticularsApi.Tests.Builders.Entities.ShipInfoTestBuilder;
using static ShipParticularsApi.Tests.Builders.Entities.ShipSatelliteTestBuilder;
using static ShipParticularsApi.Tests.Builders.Entities.ShipServiceTestBuilder;
using static ShipParticularsApi.Tests.Builders.Entities.SkTelinkCompanyShipTestBuilder;

namespace ShipParticularsApi.Tests.Entities
{
    public class ShipInfoTests
    {
        private const string FixedUserId = "TEST_USER_01";

        public class When_shipInfo_is_new
        {
            [Fact(DisplayName = "AIS 토글이 Off인 경우 ShipServices가 비어있다.")]
            public void Case1()
            {
                // Arrange
                var shipInfoDetails = new ShipInfoDetails(
                    "NEW_SHIP_KEY", "NEW_CALLSIGN", "NEW_SHIP_NAME", "FISHING", "NEW_SHIP_CODE"
                );

                var newShipInfo = ShipParticularsApi.Entities.ShipInfo.From(shipInfoDetails);

                // Act
                newShipInfo.DeactiveAisService();

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
                var shipInfoDetails = new ShipInfoDetails(
                    "NEW_SHIP_KEY", "NEW_CALLSIGN", "NEW_SHIP_NAME", "FISHING", "NEW_SHIP_CODE"
                );
                var newShipInfo = ShipParticularsApi.Entities.ShipInfo.From(shipInfoDetails);

                // Act
                newShipInfo.ActiveAisService();

                // Assert
                var expected = ShipInfo().WithShipKey(shipInfoDetails.ShipKey)
                    .WithCallsign(shipInfoDetails.Callsign)
                    .WithShipName(shipInfoDetails.ShipName)
                    .WithShipType(ShipTypes.Fishing)
                    .WithShipCode(shipInfoDetails.ShipCode)
                    .WithIsUseAis(true)
                    .WithShipServices(SatAisService(shipInfoDetails.ShipKey))
                    .WithIsService(true)
                    .Build();

                newShipInfo.Should().NotBeNull();
                newShipInfo.Should().BeEquivalentTo(expected);
            }

            [Fact(DisplayName = "GPS Toggle Off인 경우 ShipServices가 비어있다.")]
            public void Case3()
            {
                // Arrange
                var shipInfoDetails = new ShipInfoDetails(
                    "NEW_SHIP_KEY", "NEW_CALLSIGN", "NEW_SHIP_NAME", "FISHING", "NEW_SHIP_CODE"
                );
                var newShipInfo = ShipParticularsApi.Entities.ShipInfo.From(shipInfoDetails);

                // Act
                newShipInfo.DeactiveGpsService();

                // Assert
                var expected = ShipInfo().WithShipKey(shipInfoDetails.ShipKey)
                    .WithCallsign(shipInfoDetails.Callsign)
                    .WithShipName(shipInfoDetails.ShipName)
                    .WithShipType(ShipTypes.Fishing)
                    .WithShipCode(shipInfoDetails.ShipCode)
                    .WithIsService(true)
                    .Build();

                newShipInfo.Should().NotBeNull();
                newShipInfo.Should().BeEquivalentTo(expected);
            }

            [Fact(DisplayName = "GPS Toggle On이고 KT_SAT 사용하는 경우 ShipService, ShipSatellite가 추가된다")]
            public void Case4()
            {
                // Arrange
                var satelliteDetails = new SatelliteDetails("TEST_SATELLITE_ID", "KT_SAT", null);

                var shipInfoDetails = new ShipInfoDetails(
                    "NEW_SHIP_KEY", "NEW_CALLSIGN", "NEW_SHIP_NAME", "FISHING", "NEW_SHIP_CODE"
                );
                var newShipInfo = ShipParticularsApi.Entities.ShipInfo.From(shipInfoDetails);

                // Act
                newShipInfo.ActiveGpsService(satelliteDetails, FixedUserId);

                // Assert
                var expected = ShipInfo().WithShipKey(shipInfoDetails.ShipKey)
                    .WithCallsign(shipInfoDetails.Callsign)
                    .WithShipName(shipInfoDetails.ShipName)
                    .WithShipType(ShipTypes.Fishing)
                    .WithShipCode(shipInfoDetails.ShipCode)
                    .WithShipServices(KtSatService(shipInfoDetails.ShipKey))
                    .WithShipSatellite(ShipSatellite()
                        .WithShipKey(shipInfoDetails.ShipKey)
                        .WithSatelliteType(SatelliteTypes.KtSat)
                        .WithSatelliteId(satelliteDetails.SatelliteId)
                        .WithIsUseSatellite(true)
                        .WithCreateUserId(FixedUserId)
                        .Build())
                    .WithExternalShipId(satelliteDetails.SatelliteId)
                    .WithIsUseKtsat(true)
                    .WithIsService(true)
                    .Build();

                newShipInfo.Should().NotBeNull();
                newShipInfo.Should().BeEquivalentTo(expected,
                    options => options.Excluding(s => s.ShipSatellite.UpdateDateTime));
            }

            [Fact(DisplayName = "GPS Toggle On이고 SK_TELINK 사용하는 경우 ShipService, ShipSatellite, SkTelinkCompanyShip가 추가된다")]
            public void Case5()
            {
                // Arrange
                var satelliteDetails = new SatelliteDetails("SATELLITE_ID", "SK_TELINK", "UNIQUE_COMPANY_KEY");
                var shipInfoDetails = new ShipInfoDetails(
                    "NEW_SHIP_KEY", "NEW_CALLSIGN", "NEW_SHIP_NAME", "FISHING", "NEW_SHIP_CODE"
                );

                var newShipInfo = ShipParticularsApi.Entities.ShipInfo.From(shipInfoDetails);

                // Act
                newShipInfo.ActiveGpsService(satelliteDetails, FixedUserId);

                // Assert
                var expected = ShipInfo().WithShipKey(shipInfoDetails.ShipKey)
                    .WithCallsign(shipInfoDetails.Callsign)
                    .WithShipName(shipInfoDetails.ShipName)
                    .WithShipType(ShipTypes.Fishing)
                    .WithShipCode(shipInfoDetails.ShipCode)
                    .WithShipServices(KtSatService(shipInfoDetails.ShipKey))
                    .WithShipSatellite(
                        ShipSatellite()
                        .WithShipKey(shipInfoDetails.ShipKey)
                        .WithSatelliteType(SatelliteTypes.SkTelink)
                        .WithSatelliteId(satelliteDetails.SatelliteId)
                        .WithIsUseSatellite(true)
                        .WithCreateUserId(FixedUserId)
                        .Build())
                    .WithSkTelinkCompanyShip(SkTelinkCompanyShip(shipInfoDetails.ShipKey, 0L).WithCompanyName(satelliteDetails.CompanyName))
                    .WithExternalShipId(satelliteDetails.SatelliteId)
                    .WithIsUseKtsat(true)
                    .WithIsService(true)
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
                const string shipKey = "UNIQUE_SHIP_KEY";
                var shipInfoDetails = new ShipInfoDetails(
                    shipKey,
                    "UPDATE_CALLSIGN",
                    "UPDATE_SHIP_NAME",
                    "PASSENGER",
                    "UPDATE_SHIP_CODE"
                );

                var target = NoService(shipKey, 1L).Build();

                // Act
                target = target.UpdateDetails(shipInfoDetails);

                // Assert
                target.Should().BeEquivalentTo(
                    NoService(shipKey, 1L)
                        .WithCallsign(shipInfoDetails.Callsign)
                        .WithShipName(shipInfoDetails.ShipName)
                        .WithShipType(ShipTypes.Passenger)
                        .WithShipCode(shipInfoDetails.ShipCode)
                        .WithIsService(true)
                        .Build()
                );
            }

            [Fact(DisplayName = "AIS Toggle On인 경우 서비스('sat-ais')가 추가된다")]
            public void Case7()
            {
                // Arrange
                const string shipKey = "UNIQUE_SHIP_KEY";
                var target = NoService(shipKey, 1L).Build();

                // Act
                target.ActiveAisService();

                // Assert
                target.ShipServices.Should().NotBeEmpty();
                target.ShipServices.Should().ContainEquivalentOf(SatAisService(shipKey).Build());
                target.IsUseAis.Should().BeTrue();
            }

            [Fact(DisplayName = "'sat-ais' ShipService 사용 중일때, AIS Toggle On해도 아무것도 하지 않는다")]
            public void Case8()
            {
                // Arrange
                const string shipKey = "UNIQUE_SHIP_KEY";
                var target = AisOnly(shipKey, 1L).Build();

                // Act
                target.ActiveAisService();

                // Assert
                target.ShipServices.Should().HaveCount(1);
                target.ShipServices.Should().ContainEquivalentOf(SatAisService(shipKey, 1L).Build());
                target.IsUseAis.Should().BeTrue();
            }

            [Fact(DisplayName = "'sat-ais' ShipService를 사용 중이고, AIS Toggle Off하는 경우 컬렉션 길이가 0이 된다")]
            public void Case9()
            {
                // Arrange
                const string shipKey = "UNIQUE_SHIP_KEY";
                var target = AisOnly(shipKey, 1L).Build();

                // Act
                target.DeactiveAisService();

                // Assert 
                target.ShipServices.Should().BeEmpty();
                target.IsUseAis.Should().BeFalse();
            }

            [Fact(DisplayName = "등록된 ShipService가 없는 경우, AIS Toggle Off해도 아무것도 하지 않는다")]
            public void Case10()
            {
                // Arrange
                const string shipKey = "UNIQUE_SHIP_KEY";
                var target = NoService(shipKey, 1L).Build();

                // Act
                target.DeactiveAisService();

                // Assert 
                target.ShipServices.Should().BeEmpty();
                target.IsUseAis.Should().BeFalse();
            }

            [Fact(DisplayName = "등록된 ShipService가 없는 경우, GPS Toggle Off 해도 아무것도 하지 않는다")]
            public void Case11()
            {
                // Arrange
                const string shipKey = "UNIQUE_SHIP_KEY";
                var target = NoService(shipKey, 1L).Build();

                // Act
                target.DeactiveGpsService();

                // Assert
                target.ShipServices.Should().BeEmpty();

                target.ShipSatellite.Should().BeNull();
                target.IsUseKtsat.Should().BeFalse();
                target.ExternalShipId.Should().BeNull();

                target.SkTelinkCompanyShip.Should().BeNull();
            }

            // NOTE. GPS 서비스 사용시 ShipService는 고정('kt-sat')
            [Fact(DisplayName = "사용중인 GPS 서비스를 비활성화하면 관련 엔티티와 필드를 초기화한다")]
            public void Case12()
            {
                // Arrange
                const string shipKey = "UNIQUE_SHIP_KEY";
                var target = UsingSkTelink(shipKey, FixedUserId, 1L).Build();

                // Act
                target.DeactiveGpsService();

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
                const string shipKey = "UNIQUE_SHIP_KEY";
                const string satelliteId = "SATELLITE_ID";
                var target = NoService(shipKey, 1L).Build();

                // Act
                target.ActiveGpsService(new SatelliteDetails(satelliteId, "KT_SAT", null), FixedUserId);

                // Assert
                target.ShipServices.Should().ContainEquivalentOf(KtSatService(shipKey).Build());

                var expectedKtSatellite = KtSatellite(shipKey)
                        .WithSatelliteId(satelliteId)
                        .WithCreateUserId(FixedUserId)
                        .Build();

                target.ShipSatellite.Should().BeEquivalentTo(
                    expectedKtSatellite,
                    options => options.Excluding(s => s.UpdateDateTime));
                target.ExternalShipId.Should().Be(satelliteId);
                target.IsUseKtsat.Should().BeTrue();

                target.SkTelinkCompanyShip.Should().BeNull();
            }

            [Fact(DisplayName = "신규로 SK_TELINK 서비스 사용하는 경우, ShipService,ShipSatellite, SkTelinkCompanyShip가 등록된다")]
            public void Case14()
            {
                // Arrange
                const string shipKey = "UNIQUE_SHIP_KEY";
                const string satelliteId = "SATELLITE_ID";

                var target = NoService(shipKey, 1L).Build();

                // Act
                target.ActiveGpsService(
                    new SatelliteDetails(satelliteId, "SK_TELINK", "UNIQUE_COMPANY_NAME"),
                    FixedUserId
                );

                // Assert
                target.ExternalShipId.Should().Be(satelliteId);
                target.IsUseKtsat.Should().BeTrue();

                target.ShipServices.Should()
                    .ContainEquivalentOf(KtSatService(shipKey).Build());

                target.ShipSatellite.Should().BeEquivalentTo(
                    SkTelinkSatellite(shipKey).WithSatelliteId(satelliteId).WithCreateUserId(FixedUserId).Build(),
                    options => options.Excluding(s => s.UpdateDateTime)
                );

                target.SkTelinkCompanyShip.Should().NotBeNull();
            }

            [Fact(DisplayName = "KT_SAT에서 SK_TELINK로 서비스 변경하는 경우, ShipSatellite가 업데이트되고, SKTelinkCompanyShip이 추가된다")]
            public void Case15()
            {
                // Arrange
                const string shipKey = "UNIQUE_SHIP_KEY";
                const string satelliteId = "SATELLITE_ID";

                var target = UsingKtSat(shipKey, FixedUserId, 1L).Build();

                // Act
                target.ActiveGpsService(
                    new SatelliteDetails(satelliteId, "SK_TELINK", "UNIQUE_COMPANY_NAME"),
                    FixedUserId
                );

                // Assert
                target.ExternalShipId.Should().Be(satelliteId);
                target.IsUseKtsat.Should().BeTrue();

                target.ShipServices.Should()
                    .ContainEquivalentOf(KtSatService(shipKey, 1L).Build());

                target.ShipSatellite.Should().BeEquivalentTo(
                    SkTelinkSatellite(shipKey, 1L)
                        .WithSatelliteId(satelliteId)
                        .WithCreateUserId(FixedUserId)
                        .WithUpdateUserId(FixedUserId)
                        .Build()
                );

                target.SkTelinkCompanyShip.Should()
                    .BeEquivalentTo(SkTelinkCompanyShip(shipKey).Build());
            }

            [Fact(DisplayName = "SK_TELINK에서 KT_SAT로 서비스 변경하는 경우, ShipSatellite가 업데이트되고, SKTelinkCompanyShip이 제거된다")]
            public void Case16()
            {
                // Arrange
                const string shipKey = "UNIQUE_SHIP_KEY";
                const string satelliteId = "SATELLITE_ID";
                var target = UsingSkTelink(shipKey, FixedUserId, 1L).Build();

                // Act
                target.ActiveGpsService(
                    new SatelliteDetails(satelliteId, "KT_SAT", null),
                    FixedUserId
                );

                // Assert
                target.ExternalShipId.Should().Be(satelliteId);
                target.IsUseKtsat.Should().BeTrue();

                target.ShipServices.Should()
                    .ContainEquivalentOf(KtSatService(shipKey, 1L).Build());

                target.ShipSatellite.Should().BeEquivalentTo(
                    KtSatellite(shipKey, 1L)
                        .WithSatelliteId(satelliteId)
                        .WithCreateUserId(FixedUserId)
                        .WithUpdateUserId(FixedUserId)
                        .Build(),
                    options => options.Excluding(s => s.UpdateDateTime));

                target.SkTelinkCompanyShip.Should().BeNull();
            }


            [Fact(DisplayName = "SK TELINK의 CompanyName을 업데이트 한다")]
            public void Case17()
            {
                // Arrange
                const string shipKey = "UNIQUE_SHIP_KEY";
                const string satelliteId = "SATELLITE_ID";
                const string updateCompaynName = "UPDATE_COMPANY_NAME";

                var target = UsingSkTelink(shipKey, FixedUserId, 1L).Build();

                // Act
                target.ActiveGpsService(
                    new SatelliteDetails(satelliteId, "SK_TELINK", updateCompaynName),
                    FixedUserId
                );

                // Assert
                target.SkTelinkCompanyShip.Should()
                    .BeEquivalentTo(SkTelinkCompanyShip(shipKey, 1L).WithCompanyName(updateCompaynName).Build());
            }

            [Fact(DisplayName = "SK TELINK의 CompanyName 업데이트시 다른 엔티티는 변경되지 않는다")]
            public void Case18()
            {
                //Arrange
                const string shipKey = "UNIQUE_SHIP_KEY";
                const string satelliteId = "SATELLITE_ID";
                var target = UsingSkTelink(shipKey, FixedUserId, 1L).Build();

                // Act
                target.ActiveGpsService(
                    new SatelliteDetails(satelliteId, "SK_TELINK", "UPDATE_COMPANY_NAME"),
                    FixedUserId
                );

                // Assert
                target.ShipServices.Should()
                    .ContainEquivalentOf(KtSatService(shipKey, 1L).Build());

                var expectedSkTelinkSatellite = SkTelinkSatellite(shipKey, 1L)
                        .WithSatelliteId(satelliteId)
                        .WithCreateUserId(FixedUserId)
                        .WithUpdateUserId(FixedUserId)
                        .Build();

                target.ShipSatellite.Should().BeEquivalentTo(expectedSkTelinkSatellite);
            }
        }

    }
}
