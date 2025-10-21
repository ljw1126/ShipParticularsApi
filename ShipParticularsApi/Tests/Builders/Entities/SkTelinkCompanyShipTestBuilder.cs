using ShipParticularsApi.Entities;

namespace ShipParticularsApi.Tests.Builders.Entities
{
    public class SkTelinkCompanyShipTestBuilder
    {
        private const long NEW_ID = 0L;
        private const string DEFAULT_COMPANY_NAME = "UNIQUE_COMPANY_NAME";

        private long _Id;
        private string _ShipKey;
        private string _CompanyName = "UNIQUE_COMPANY_NAME";

        public static SkTelinkCompanyShipTestBuilder SkTelinkCompanyShip()
        {
            return new SkTelinkCompanyShipTestBuilder();
        }

        public static SkTelinkCompanyShip SkTelinkCompanyShip(long id, string shipKey)
        {
            return SkTelinkCompanyShip(id, shipKey, DEFAULT_COMPANY_NAME);
        }

        public static SkTelinkCompanyShip SkTelinkCompanyShip(string shipKey)
        {
            return SkTelinkCompanyShip(NEW_ID, shipKey, DEFAULT_COMPANY_NAME);
        }

        public static SkTelinkCompanyShip SkTelinkCompanyShip(string shipKey, string companyName)
        {
            return SkTelinkCompanyShip(NEW_ID, shipKey, companyName);
        }

        public static SkTelinkCompanyShip SkTelinkCompanyShip(long id, string shipKey, string companyName)
        {
            return SkTelinkCompanyShip()
                .WithId(id)
                .WithShipKey(shipKey)
                .WithCompanyName(companyName)
                .Build();
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
            return new()
            {
                Id = _Id,
                ShipKey = _ShipKey,
                CompanyName = _CompanyName
            };
        }
    }
}
