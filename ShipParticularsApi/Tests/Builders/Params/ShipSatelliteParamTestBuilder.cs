using ShipParticularsApi.Services.Dtos.Params;

namespace ShipParticularsApi.Tests.Builders.Params
{
    public class ShipSatelliteParamTestBuilder
    {
        private string _SatelliteType = "NONE";
        private string _SatelliteId = "SATELLITE_ID";

        public static ShipSatelliteParamTestBuilder ShipSatelliteParam()
        {
            return new ShipSatelliteParamTestBuilder();
        }

        public static ShipSatelliteParamTestBuilder KtSatelliteParam()
        {
            return ShipSatelliteParam()
                .WithSatelliteType("KT_SAT");
        }

        public static ShipSatelliteParamTestBuilder SkTelinkSatelliteParam()
        {
            return ShipSatelliteParam()
                .WithSatelliteType("SK_TELINK");
        }

        public ShipSatelliteParamTestBuilder WithSatelliteType(string satelliteType)
        {
            this._SatelliteType = satelliteType;
            return this;
        }

        public ShipSatelliteParamTestBuilder WithSatelliteId(string satelliteId)
        {
            this._SatelliteId = satelliteId;
            return this;
        }

        public ShipSatelliteParam Build()
        {
            return new()
            {
                SatelliteId = _SatelliteId,
                SatelliteType = _SatelliteType
            };
        }
    }
}
