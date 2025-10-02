using ShipParticularsApi.Entities;

namespace ShipParticularsApi.Tests.Builders
{
    public class SkTelinkCompanyShipTestBuilder
    {
        private long _Id;
        private string _ShipKey;

        public static SkTelinkCompanyShipTestBuilder SkTelinkCompanyShip()
        {
            return new SkTelinkCompanyShipTestBuilder();
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

        public SkTelinkCompanyShip Build()
        {
            return new()
            {
                Id = _Id,
                ShipKey = _ShipKey
            };
        }
    }
}
