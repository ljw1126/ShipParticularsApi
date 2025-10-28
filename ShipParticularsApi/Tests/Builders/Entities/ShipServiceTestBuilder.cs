using ShipParticularsApi.Entities;

namespace ShipParticularsApi.Tests.Builders.Entities
{
    public class ShipServiceTestBuilder
    {
        private long _Id;
        private string _ShipKey;
        private ServiceNameTypes? _ServiceName;
        private bool _IsCompleted = true;

        public static ShipServiceTestBuilder ShipService()
        {
            return new ShipServiceTestBuilder();
        }

        public static ShipServiceTestBuilder SatAisService(string shipKey, long id = 0L)
        {
            return SatAisService()
                .WithId(id)
                .WithShipKey(shipKey)
                .WithIsCompleted(true);
        }

        public static ShipServiceTestBuilder SatAisService()
        {
            return ShipService()
                .WithServiceName(ServiceNameTypes.SatAis)
                .WithIsCompleted(true);
        }

        public static ShipServiceTestBuilder KtSatService(string shipKey, long id = 0L)
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
            return new(_Id, _ShipKey, _ServiceName, _IsCompleted);
        }
    }
}
