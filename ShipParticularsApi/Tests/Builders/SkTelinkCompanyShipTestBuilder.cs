using ShipParticularsApi.Entities;

namespace ShipParticularsApi.Tests.Builders
{
    public class SkTelinkCompanyShipTestBuilder
    {
        private long _Id;
        private string _ShipKey;
        private string _CompanyName;

        public static SkTelinkCompanyShipTestBuilder SkTelinkCompanyShip()
        {
            return new SkTelinkCompanyShipTestBuilder();
        }

        public static SkTelinkCompanyShip SkTelinkCompanyShip(string shipKey, string companyName)
        {
            return SkTelinkCompanyShip()
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
