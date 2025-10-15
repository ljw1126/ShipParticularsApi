using ShipParticularsApi.Entities;

namespace ShipParticularsApi.Tests.Builders
{
    public class SkTelinkCompanyShipTestBuilder
    {
        private const long NEW_ID = 0L;

        private long _Id;
        private string _ShipKey;
        private string _CompanyName;

        public static SkTelinkCompanyShipTestBuilder SkTelinkCompanyShip()
        {
            return new SkTelinkCompanyShipTestBuilder();
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
