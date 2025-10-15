using ShipParticularsApi.Entities;

namespace ShipParticularsApi.Tests.Builders
{
    public class ShipServiceTestBuilder
    {
        private const long NEW_ID = 0L;

        private long _Id;
        private string _ShipKey;
        private ServiceNameTypes? _ServiceName;
        private bool _IsCompleted;

        public static ShipServiceTestBuilder ShipService()
        {
            return new ShipServiceTestBuilder();
        }

        public static ShipService SatAisService(string shipKey)
        {
            return SatAisService(NEW_ID, shipKey);
        }

        public static ShipService SatAisService(long id, string shipKey)
        {
            return ShipService()
                .WithId(id)
                .WithShipKey(shipKey)
                .WithServiceName(ServiceNameTypes.SatAis)
                .WithIsCompleted(true)
                .Build();
        }

        public static ShipService KtSatService(string shipKey)
        {
            return KtSatService(NEW_ID, shipKey);
        }

        public static ShipService KtSatService(long id, string shipKey)
        {
            return ShipService()
                .WithId(id)
                .WithShipKey(shipKey)
                .WithServiceName(ServiceNameTypes.KtSat)
                .WithIsCompleted(true)
                .Build();
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

        public ShipServiceTestBuilder WithServiceName(ServiceNameTypes serviceName)
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
