using ShipParticularsApi.Entities;

namespace ShipParticularsApi.Tests.Builders
{
    public class ShipServiceTestBuilder
    {
        private long _Id;
        private string _ShipKey;
        private string? _ServiceName;
        private bool _IsCompleted;

        public static ShipServiceTestBuilder ShipService()
        {
            return new ShipServiceTestBuilder();
        }

        public ShipServiceTestBuilder WithId(long id)
        {
            _Id = id;
            return this;
        }

        public ShipServiceTestBuilder WithShipKey(string shipKey)
        {
            _ShipKey = shipKey;
            return this;
        }

        public ShipServiceTestBuilder WithServiceName(string serviceName)
        {
            _ServiceName = serviceName;
            return this;
        }

        public ShipServiceTestBuilder WithIsCompleted(bool isCompleted)
        {
            _IsCompleted = isCompleted;
            return this;
        }

        public ShipService Build()
        {
            return new()
            {
                Id = _Id,
                ShipKey = _ShipKey,
                ServiceName = _ServiceName,
                IsCompleted = _IsCompleted
            };
        }
    }
}
