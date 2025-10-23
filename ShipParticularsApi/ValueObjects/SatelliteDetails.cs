using ShipParticularsApi.Services.Dtos.Params;

namespace ShipParticularsApi.ValueObjects
{
    public record SatelliteDetails
    {
        public string? SatelliteId { get; }
        public string? SatelliteType { get; }
        public string? CompanyName { get; }

        public SatelliteDetails(string? satelliteId, string? satelliteType, string? companyName)
        {
            SatelliteId = satelliteId;
            SatelliteType = satelliteType;
            CompanyName = companyName;
        }

        public static SatelliteDetails From(ShipParticularsParam param)
        {
            return new SatelliteDetails(
                param.ShipSatelliteParam?.SatelliteId,
                param.ShipSatelliteParam?.SatelliteType,
                param.SkTelinkCompanyShipParam?.CompanyName
            );
        }
    }
}
