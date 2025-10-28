using ShipParticularsApi.Entities;

namespace ShipParticularsApi.Tests.Builders.Entities
{
    public class ReplaceShipNameTestBuilder
    {
        private long _Id;
        private string _ShipKey;
        private string _ReplacedShipName;

        public static ReplaceShipNameTestBuilder ReplaceShipName()
        {
            return new ReplaceShipNameTestBuilder();
        }

        public ReplaceShipNameTestBuilder WithId(long id)
        {
            _Id = id;
            return this;
        }

        public ReplaceShipNameTestBuilder WithShipKey(string shipKey)
        {
            _ShipKey = shipKey;
            return this;
        }

        public ReplaceShipNameTestBuilder WithReplaceShipName(string replaceShipName)
        {
            _ReplacedShipName = replaceShipName;
            return this;
        }

        public ReplaceShipName Build()
        {
            return new(_Id, _ShipKey, _ReplacedShipName);
        }
    }
}
