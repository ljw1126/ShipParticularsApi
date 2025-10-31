using ShipParticularsApi.Entities;
using ShipParticularsApi.Entities.Enums;

namespace ShipParticularsApi.Tests.Tests.Builders.Entities
{
    public class ShipSatelliteTestBuilder
    {
        private const string DEFAULT_SATELLITE_ID = "SATELIITE_ID";

        private long _Id;
        private string _ShipKey = "SHIP_KEY";
        private SatelliteTypes _SatelliteType = SatelliteTypes.KtSat;
        private string? _SatelliteId = DEFAULT_SATELLITE_ID;
        private bool _IsUseSatellite = true;
        private string? _CreateUserId;
        private DateTime _CreateDateTime;
        private string? _UpdateUserId;
        private DateTime _UpdateDateTime;

        public static ShipSatelliteTestBuilder ShipSatellite()
        {
            return new ShipSatelliteTestBuilder();
        }

        public static ShipSatelliteTestBuilder KtSatellite(string shipKey, long id = 0L)
        {
            return KtSatellite()
                .WithId(id)
                .WithShipKey(shipKey);
        }

        public static ShipSatelliteTestBuilder KtSatellite()
        {
            return ShipSatellite()
                .WithSatelliteType(SatelliteTypes.KtSat)
                .WithIsUseSatellite(true);
        }

        public static ShipSatelliteTestBuilder SkTelinkSatellite(string shipKey, long id = 0L)
        {
            return SkTelinkSatellite()
                .WithId(id)
                .WithShipKey(shipKey);
        }

        public static ShipSatelliteTestBuilder SkTelinkSatellite()
        {
            return ShipSatellite()
                .WithSatelliteType(SatelliteTypes.SkTelink)
                .WithIsUseSatellite(true);
        }

        public ShipSatelliteTestBuilder WithId(long id)
        {
            _Id = id;
            return this;
        }

        public ShipSatelliteTestBuilder WithShipKey(string shipKey)
        {
            _ShipKey = shipKey;
            return this;
        }

        public ShipSatelliteTestBuilder WithSatelliteType(SatelliteTypes satelliteType)
        {
            _SatelliteType = satelliteType;
            return this;
        }

        public ShipSatelliteTestBuilder WithSatelliteId(string satelliteId)
        {
            _SatelliteId = satelliteId;
            return this;
        }

        public ShipSatelliteTestBuilder WithIsUseSatellite(bool isUseSatellite)
        {
            _IsUseSatellite = isUseSatellite;
            return this;
        }

        public ShipSatelliteTestBuilder WithCreateUserId(string createUserId)
        {
            _CreateUserId = createUserId;
            return this;
        }

        public ShipSatelliteTestBuilder WithUpdateUserId(string updateUserId)
        {
            _UpdateUserId = updateUserId;
            return this;
        }

        public ShipSatellite Build()
        {
            return new(
                _Id,
                _ShipKey,
                _SatelliteType,
                _SatelliteId,
                _IsUseSatellite,
                _CreateUserId,
                _UpdateUserId
            );
        }
    }
}
