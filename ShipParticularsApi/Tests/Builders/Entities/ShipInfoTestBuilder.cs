using ShipParticularsApi.Entities;
using ShipParticularsApi.Entities.Enums;

namespace ShipParticularsApi.Tests.Builders.Entities
{
    public class ShipInfoTestBuilder
    {
        private long _Id;
        private string _ShipKey = "SHIP_KEY";
        private string _Callsign = "CALLSIGN";
        private string _ShipName = "SHIP_NAME";
        private ShipTypes? _ShipType = ShipTypes.Default;
        private string? _ShipCode;
        private string? _ExternalShipId;
        private bool? _IsUseKtsat;
        private bool _IsService = true;
        private bool _IsUseAis;

        private ReplaceShipName _ReplaceShipName = null;
        private ShipModelTest _ShipModelTest = null;
        private ShipSatellite _ShipSatellite = null;
        private ICollection<ShipService> _ShipServices = [];
        private SkTelinkCompanyShip _SkTelinkCompanyShip = null;

        public static ShipInfoTestBuilder UsingKtSat(string shipKey, string userId, long id = 1L)
        {
            const string satelliteId = "SATELITE_ID";

            return NoService(shipKey, id)
                .WithShipServices(ShipServiceTestBuilder.KtSatService(id, shipKey))
                .WithShipSatellite(ShipSatelliteTestBuilder.KtSatellite()
                    .WithId(id)
                    .WithShipKey(shipKey)
                    .WithSatelliteId(satelliteId)
                    .WithCreateUserId(userId)
                    .Build()
                )
                .WithIsUseKtsat(true)
                .WithExternalShipId(satelliteId)
                .WithIsUseAis(false);
        }

        public static ShipInfoTestBuilder UsingSkTelink(string shipKey, string userId, long id = 1L)
        {
            const string satelliteId = "SATELITE_ID";

            return NoService(shipKey, id)
                .WithShipServices(ShipServiceTestBuilder.KtSatService(id, shipKey))
                .WithShipSatellite(ShipSatelliteTestBuilder.SkTelinkSatellite()
                        .WithId(id)
                        .WithShipKey(shipKey)
                        .WithSatelliteId(satelliteId)
                        .WithCreateUserId(userId)
                        .Build())
                .WithSkTelinkCompanyShip(SkTelinkCompanyShipTestBuilder.SkTelinkCompanyShip(id, shipKey))
                .WithIsUseKtsat(true)
                .WithExternalShipId(satelliteId)
                .WithIsUseAis(false);
        }

        public static ShipInfoTestBuilder AisOnly(string shipKey, long id = 1L)
        {
            return NoService(shipKey, id)
                .WithIsUseAis(true)
                .WithShipServices(ShipServiceTestBuilder.SatAisService(id, shipKey));
        }

        public static ShipInfoTestBuilder NoService(string shipKey, long id = 1L)
        {
            return ExsitingBase(id, shipKey)
                .WithIsService(true)
                .WithIsUseAis(false)
                .WithIsUseKtsat(false);
        }

        private static ShipInfoTestBuilder ExsitingBase(long id, string shipKey)
        {
            return ShipInfo()
                .WithId(id)
                .WithShipKey(shipKey);
        }

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
