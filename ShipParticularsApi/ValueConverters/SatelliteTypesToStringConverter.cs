using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShipParticularsApi.Entities;

namespace ShipParticularsApi.ValueConverters
{
    public class SatelliteTypesToStringConverter : ValueConverter<SatelliteTypes, string>
    {
        public SatelliteTypesToStringConverter() : base(
            v => SatelliteTypesToString(v),
            v => StringToSatelliteTypes(v))
        {
        }

        private static String SatelliteTypesToString(SatelliteTypes satelliteTypes)
        {
            return satelliteTypes switch
            {
                SatelliteTypes.None => "NONE",
                SatelliteTypes.KtSat => "KT_SAT",
                SatelliteTypes.SkTelink => "SK_TELINK",
                SatelliteTypes.SynerSat => "SYNER_SAT",
                _ => throw new ArgumentException($"Invalid SatelliteTypes '{satelliteTypes}'")
            };
        }

        private static SatelliteTypes StringToSatelliteTypes(String value)
        {
            return value switch
            {
                "NONE" => SatelliteTypes.None,
                "KT_SAT" => SatelliteTypes.KtSat,
                "SK_TELINK" => SatelliteTypes.SkTelink,
                "SYNER_SAT" => SatelliteTypes.SynerSat,
                _ => throw new ArgumentException($"Invalid string value '{value}' for SatelliteTypes enum")
            };
        }
    }
}
