using ShipParticularsApi.Entities;

namespace ShipParticularsApi.Tests.Builders.Entities
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

        public static ShipServiceTestBuilder SatAisService(string shipKey)
        {
            return SatAisService(NEW_ID, shipKey);
        }

        public static ShipServiceTestBuilder SatAisService(long id, string shipKey)
        {
            return SatAisService()
                .WithId(id)
                .WithShipKey(shipKey);
        }

        public static ShipServiceTestBuilder SatAisService()
        {
            return ShipService()
                .WithServiceName(ServiceNameTypes.SatAis)
                .WithIsCompleted(true);
        }

        public static ShipServiceTestBuilder KtSatService(string shipKey)
        {
            return KtSatService(NEW_ID, shipKey);
        }

        public static ShipServiceTestBuilder KtSatService(long id, string shipKey)
        {
            return KtSatService()
                .WithId(id)
                .WithShipKey(shipKey);
        }

        public static ShipServiceTestBuilder KtSatService()
        {
            return ShipService()
                .WithServiceName(ServiceNameTypes.KtSat)
                .WithIsCompleted(true);
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
