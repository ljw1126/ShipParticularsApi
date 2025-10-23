using ShipParticularsApi.Exceptions;

namespace ShipParticularsApi.Entities.Enums
{
    public static class SatelliteTypesConverter
    {
        public static SatelliteTypes ToEnum(string value)
        {
            return value.ToUpperInvariant() switch
            {
                "NONE" => SatelliteTypes.None,
                "KT_SAT" => SatelliteTypes.KtSat,
                "SK_TELINK" => SatelliteTypes.SkTelink,
                "SYNER_SAT" => SatelliteTypes.SynerSat,
                _ => throw new InvalidOperationException($"위성 타입 변환 실패: 허용되지 않은 문자열 값 '{value}'")
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
                _ => throw new InvalidOperationException($"유효하지 않은 위성 타입 값 입니다. '{types}'")
            };
        }

        public static SatelliteTypes ParseFromRequest(string value)
        {
            try
            {
                return ToEnum(value);
            }
            catch (InvalidOperationException)
            {
                throw new BadRequestException($"유효하지 않은 위성 타입입니다. 입력 값: '{value}'");
            }
        }
    }
}
