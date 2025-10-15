using ShipParticularsApi.Entities;

namespace ShipParticularsApi.Tests.Builders
{
    public class ShipSatelliteTestBuilder
    {
        private const long NEW_ID = 0L;

        private long _Id;
        private string _ShipKey;
        private SatelliteTypes _SatelliteType;
        private string? _SatelliteId;
        private bool _IsUseSatellite;
        private string? _CreateUserId;
        private DateTime _CreateDateTime;
        private string _UpdateUserId;
        private DateTime _UpdateDateTime;

        public static ShipSatelliteTestBuilder ShipSatellite()
        {
            return new ShipSatelliteTestBuilder();
        }

        public static ShipSatellite KtSatellite(string shipKey, string satelliteId)
        {
            return KtSatellite(NEW_ID, shipKey, satelliteId);
        }

        public static ShipSatellite KtSatellite(long id, string shipKey, string satelliteId)
        {
            return ShipSatellite()
                .WithId(id)
                .WithShipKey(shipKey)
                .WithSatelliteType(SatelliteTypes.KtSat)
                .WithSatelliteId(satelliteId)
                .WithIsUseSatellite(true)
                .Build();

        }

        public static ShipSatellite SkTelinkSatellite(string shipKey, string satelliteId)
        {
            return SkTelinkSatellite(0L, shipKey, satelliteId);
        }

        public static ShipSatellite SkTelinkSatellite(long id, string shipKey, string satelliteId)
        {
            return ShipSatellite()
                .WithId(id)
                .WithShipKey(shipKey)
                .WithSatelliteType(SatelliteTypes.SkTelink)
                .WithSatelliteId(satelliteId)
                .WithIsUseSatellite(true)
                .Build();
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

        public ShipSatelliteTestBuilder WithCreateDateTime(DateTime createDateTime)
        {
            _CreateDateTime = createDateTime;
            return this;
        }

        public ShipSatelliteTestBuilder WithUpdateUserId(string updateUserId)
        {
            _UpdateUserId = updateUserId;
            return this;
        }

        public ShipSatelliteTestBuilder WithUpdateDateTime(DateTime updateDateTime)
        {
            _UpdateDateTime = updateDateTime;
            return this;
        }

        public ShipSatellite Build()
        {
            return new()
            {
                Id = _Id,
                ShipKey = _ShipKey,
                SatelliteType = _SatelliteType,
                SatelliteId = _SatelliteId,
                IsUseSatellite = _IsUseSatellite,
                CreateUserId = _CreateUserId,
                CreateDateTime = _CreateDateTime,
                UpdateUserId = _UpdateUserId,
                UpdateDateTime = _UpdateDateTime
            };
        }
    }
}
