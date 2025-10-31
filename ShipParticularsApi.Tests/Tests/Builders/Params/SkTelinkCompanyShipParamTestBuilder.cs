using ShipParticularsApi.Services.Dtos.Params;

namespace ShipParticularsApi.Tests.Tests.Builders.Params
{
    public class SkTelinkCompanyShipParamTestBuilder
    {
        private string _CompanyName = "UNIQUE_COMPANY_NAME";

        public static SkTelinkCompanyShipParamTestBuilder SkTelinkCompanyShipParam()
        {
            return new SkTelinkCompanyShipParamTestBuilder();
        }

        public SkTelinkCompanyShipParamTestBuilder WithCompanyName(string companyName)
        {
            _CompanyName = companyName;
            return this;
        }

        public SkTelinkCompanyShipParam Build()
        {
            return new()
            {
                CompanyName = _CompanyName
            };
        }
    }
}
