using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShipParticularsApi.Entities.Enums;

namespace ShipParticularsApi.ValueConverters
{
    public class SatelliteTypesToStringConverter : ValueConverter<SatelliteTypes, string>
    {
        public SatelliteTypesToStringConverter() : base(
            v => SatelliteTypesConverter.ToString(v),
            v => SatelliteTypesConverter.ToSatelliteTypes(v))
        {
        }
    }
}
