using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShipParticularsApi.Entities;

namespace ShipParticularsApi.ValueConverters
{
    public class ServiceNameToStringConverter : ValueConverter<ServiceNameTypes, string>
    {
        public ServiceNameToStringConverter() : base(
            v => ServiceNameTypesToString(v),
            v => StringToServiceNameType(v))
        {
        }

        private static string ServiceNameTypesToString(ServiceNameTypes serviceNameTypes)
        {
            return serviceNameTypes switch
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
                _ => throw new ArgumentException($"Invalid ServiceNameTypes '{serviceNameTypes}'")
            };
        }

        private static ServiceNameTypes StringToServiceNameType(string value)
        {
            return value switch
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
                _ => throw new ArgumentException($"Invalid string value '{value}' for ServiceNameTypes enum")
            };
        }
    }
}
