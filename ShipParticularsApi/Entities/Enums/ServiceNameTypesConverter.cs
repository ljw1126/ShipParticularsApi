using ShipParticularsApi.Exceptions;

namespace ShipParticularsApi.Entities.Enums
{
    public static class ServiceNameTypesConverter
    {
        public static ServiceNameTypes ToEnum(string value)
        {
            return value.ToLowerInvariant() switch
            {
                "pdca" => ServiceNameTypes.Pdca,
                "me-shop-test" => ServiceNameTypes.MeShopTest,
                "eu-mrv" => ServiceNameTypes.EuMrv,
                "model-test" => ServiceNameTypes.ModelTest,
                "noon-report" => ServiceNameTypes.NoonReport,
                "sat-ais" => ServiceNameTypes.SatAis,
                "cctv" => ServiceNameTypes.Cctv,
                "imo-dcs" => ServiceNameTypes.ImoDcs,
                "kt-sat" => ServiceNameTypes.KtSat,
                "data-logger" => ServiceNameTypes.DataLogger,
                "sea-trial" => ServiceNameTypes.SeaTrial,
                "ship-particular" => ServiceNameTypes.ShipParticular,
                "ams" => ServiceNameTypes.Ams,
                "small-logger" => ServiceNameTypes.SmallLogger,
                _ => throw new InvalidOperationException($"서비스명 타입 변환 실패: 허용되지 않은 문자열 값 '{value}'")
            };
        }

        public static string ToString(ServiceNameTypes types)
        {
            return types switch
            {
                ServiceNameTypes.Pdca => "pdca",
                ServiceNameTypes.MeShopTest => "me-shop-test",
                ServiceNameTypes.EuMrv => "eu-mrv",
                ServiceNameTypes.ModelTest => "model-test",
                ServiceNameTypes.NoonReport => "noon-report",
                ServiceNameTypes.SatAis => "sat-ais",
                ServiceNameTypes.Cctv => "cctv",
                ServiceNameTypes.ImoDcs => "imo-dcs",
                ServiceNameTypes.KtSat => "kt-sat",
                ServiceNameTypes.DataLogger => "data-logger",
                ServiceNameTypes.SeaTrial => "sea-trial",
                ServiceNameTypes.ShipParticular => "ship-particular",
                ServiceNameTypes.Ams => "ams",
                ServiceNameTypes.SmallLogger => "small-logger",
                _ => throw new InvalidOperationException($"유효하지 않은 서비스명 타입 값 입니다. '{types}'")
            };
        }

        public static ServiceNameTypes ParseFromRequest(string value)
        {
            try
            {
                return ToEnum(value);
            }
            catch (InvalidOperationException)
            {
                throw new BadRequestException($"유효하지 않은 서비스명 입니다. 입력 값: '{value}'");
            }
        }

    }
}
