using ShipParticularsApi.Entities;

namespace ShipParticularsApi.Tests.Builders.Entities
{
    public class SkTelinkCompanyShipTestBuilder
    {
        private const string DEFAULT_COMPANY_NAME = "UNIQUE_COMPANY_NAME";

        private long _Id;
        private string _ShipKey;
        private string _CompanyName = DEFAULT_COMPANY_NAME;

        public static SkTelinkCompanyShipTestBuilder SkTelinkCompanyShip()
        {
            return new SkTelinkCompanyShipTestBuilder();
        }

        public static SkTelinkCompanyShipTestBuilder SkTelinkCompanyShip(string shipKey, long id = 0L)
        {
            return SkTelinkCompanyShip()
                .WithId(id)
                .WithShipKey(shipKey);
        }

        public SkTelinkCompanyShipTestBuilder WithId(long id)
        {
            _Id = id;
            return this;
        }

        public SkTelinkCompanyShipTestBuilder WithShipKey(string shipKey)
        {
            _ShipKey = shipKey;
            return this;
        }

        public SkTelinkCompanyShipTestBuilder WithCompanyName(string companyName)
        {
            _CompanyName = companyName;
            return this;
        }

        public SkTelinkCompanyShip Build()
        {
            return new(_Id, _ShipKey, _CompanyName);
        }
    }
}
