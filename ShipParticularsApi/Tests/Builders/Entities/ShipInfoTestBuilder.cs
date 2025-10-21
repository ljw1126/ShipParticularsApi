using ShipParticularsApi.Entities;
using ShipParticularsApi.Entities.Enums;

namespace ShipParticularsApi.Tests.Builders.Entities
{
    public class ShipInfoTestBuilder
    {
        private long _Id;
        private string _ShipKey;
        private string _Callsign = "TEST_CALLSIGN";
        private string _ShipName = "TEST_SHIPNAME";
        private ShipTypes? _ShipType;
        private string? _ShipCode;
        private string? _ExternalShipId;
        private bool? _IsUseKtsat;
        private bool? _IsService;
        private bool _IsUseAis;

        private ReplaceShipName _ReplaceShipName = null;
        private ShipModelTest _ShipModelTest = null;
        private ShipSatellite _ShipSatellite = null;
        private ICollection<ShipService> _ShipServices = [];
        private SkTelinkCompanyShip _SkTelinkCompanyShip = null;

        public static ShipInfoTestBuilder ShipInfo()
        {
            return new ShipInfoTestBuilder();
        }

        public ShipInfoTestBuilder WithId(long id)
        {
            _Id = id;
            return this;
        }

        public ShipInfoTestBuilder WithShipKey(string shipKey)
        {
            _ShipKey = shipKey;
            return this;
        }

        public ShipInfoTestBuilder WithCallsign(string callsign)
        {
            _Callsign = callsign;
            return this;
        }

        public ShipInfoTestBuilder WithShipName(string shipName)
        {
            _ShipName = shipName;
            return this;
        }

        public ShipInfoTestBuilder WithShipType(ShipTypes shipType)
        {
            _ShipType = shipType;
            return this;
        }

        public ShipInfoTestBuilder WithShipCode(string shipCode)
        {
            _ShipCode = shipCode;
            return this;
        }

        public ShipInfoTestBuilder WithExternalShipId(string externalShipId)
        {
            _ExternalShipId = externalShipId;
            return this;
        }

        public ShipInfoTestBuilder WithIsUseKtsat(bool isUseKtsat)
        {
            _IsUseKtsat = isUseKtsat;
            return this;
        }

        public ShipInfoTestBuilder WithIsService(bool isService)
        {
            _IsService = isService;
            return this;
        }

        public ShipInfoTestBuilder WithIsUseAis(bool isUseAis)
        {
            _IsUseAis = isUseAis;
            return this;
        }

        // NOTE : deprecated
        public ShipInfoTestBuilder WithReplaceShipName(ReplaceShipName replaceShipName)
        {
            _ReplaceShipName = replaceShipName;
            return this;
        }

        public ShipInfoTestBuilder WithReplaceShipName(ReplaceShipNameTestBuilder builder)
        {
            _ReplaceShipName = builder.Build();
            return this;
        }

        public ShipInfoTestBuilder WithShipModelTest(ShipModelTest shipModelTest)
        {
            _ShipModelTest = shipModelTest;
            return this;
        }

        public ShipInfoTestBuilder WithShipSatellite(ShipSatellite shipSatellite)
        {
            _ShipSatellite = shipSatellite;
            return this;
        }

        // NOTE : deprecated
        public ShipInfoTestBuilder WithShipServices(params ShipService[] shipServices)
        {
            foreach (var item in shipServices)
            {
                _ShipServices.Add(item);
            }

            return this;
        }

        public ShipInfoTestBuilder WithShipServices(params ShipServiceTestBuilder[] builders)
        {
            foreach (var item in builders)
            {
                _ShipServices.Add(item.Build());
            }

            return this;
        }

        public ShipInfoTestBuilder WithSkTelinkCompanyShip(SkTelinkCompanyShip skTelinkCompanyShip)
        {
            _SkTelinkCompanyShip = skTelinkCompanyShip;
            return this;
        }

        public ShipInfo Build()
        {
            return new ShipInfo
            {
                Id = _Id,
                ShipKey = _ShipKey,
                Callsign = _Callsign,
                ShipName = _ShipName,
                ShipType = _ShipType,
                ShipCode = _ShipCode,
                ExternalShipId = _ExternalShipId,
                IsUseKtsat = _IsUseKtsat,
                IsService = _IsService,
                IsUseAis = _IsUseAis,
                ReplaceShipName = _ReplaceShipName,
                ShipModelTest = _ShipModelTest,
                ShipSatellite = _ShipSatellite,
                ShipServices = _ShipServices,
                SkTelinkCompanyShip = _SkTelinkCompanyShip
            };
        }
    }
}
