namespace ShipParticularsApi.Entities.Enums
{
    public static class SatelliteTypesConverter
    {
        public static SatelliteTypes ToSatelliteTypes(string value)
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

        public static string ToString(SatelliteTypes types)
        {
            return types switch
            {
                SatelliteTypes.None => "NONE",
                SatelliteTypes.KtSat => "KT_SAT",
                SatelliteTypes.SkTelink => "SK_TELINK",
                SatelliteTypes.SynerSat => "SYNER_SAT",
                _ => throw new ArgumentException($"Invalid SatelliteTypes '{types}'")
            };
        }
    }
}
