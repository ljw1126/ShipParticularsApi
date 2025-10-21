using ShipParticularsApi.Services.Dtos.Params;

namespace ShipParticularsApi.Tests.Builders.Params
{
    public class ShipParticularParamTestBuilder
    {
        private bool _IsAisToggleOn = false;
        private bool _IsGPSToggleOn = false;

        private string _ShipKey = "SHIP_KEY";
        private string _Callsign = "CALLSIGN";
        private string _ShipName = "SHIP_NAME";
        private string _ShipType = "-"; // DEFAULT 
        private string _ShipCode = "SHIP_CODE";

        private ShipSatelliteParam _ShipSatelliteParam = null;
        private SkTelinkCompanyShipParam _SkTelinkCompanyShipParam = null;
        private ReplaceShipNameParam _ReplaceShipNameParam = null;
        private ShipModelTestParam _ShipModelTestParam = null;

        private ShipParticularParamTestBuilder()
        {
        }

        private ShipParticularParamTestBuilder(ShipParticularParamTestBuilder copy)
        {
            _IsAisToggleOn = copy._IsAisToggleOn;
            _IsGPSToggleOn = copy._IsGPSToggleOn;
            _ShipKey = copy._ShipKey;
            _Callsign = copy._Callsign;
            _ShipName = copy._ShipName;
            _ShipType = copy._ShipType;
            _ShipCode = copy._ShipCode;
            _ShipSatelliteParam = copy._ShipSatelliteParam;
            _SkTelinkCompanyShipParam = copy._SkTelinkCompanyShipParam;
            _ReplaceShipNameParam = copy._ReplaceShipNameParam;
            _ShipModelTestParam = copy._ShipModelTestParam;
        }

        public static ShipParticularParamTestBuilder ShipParticularsParam()
        {
            return new ShipParticularParamTestBuilder();
        }

        public ShipParticularParamTestBuilder but()
        {
            return new ShipParticularParamTestBuilder(this);
        }

        public ShipParticularParamTestBuilder WithIsAisToggleOn(bool isAisToggleOn)
        {
            this._IsAisToggleOn = isAisToggleOn;
            return this;
        }

        public ShipParticularParamTestBuilder WithIsGPSToggleOn(bool isGPSToggleOn)
        {
            this._IsGPSToggleOn = isGPSToggleOn;
            return this;
        }

        public ShipParticularParamTestBuilder WithShipKey(string shipKey)
        {
            this._ShipKey = shipKey;
            return this;
        }

        public ShipParticularParamTestBuilder WithCallsign(string callsign)
        {
            this._Callsign = callsign;
            return this;
        }

        public ShipParticularParamTestBuilder WithShipName(string shipName)
        {
            this._ShipName = shipName;
            return this;
        }

        public ShipParticularParamTestBuilder WithShipType(string shipType)
        {
            this._ShipType = shipType;
            return this;
        }

        public ShipParticularParamTestBuilder WithShipCode(string shipCode)
        {
            this._ShipCode = shipCode;
            return this;
        }

        public ShipParticularParamTestBuilder WithShipSatelliteParam(ShipSatelliteParamTestBuilder builder)
        {
            this._ShipSatelliteParam = builder.Build();
            return this;
        }

        public ShipParticularParamTestBuilder WithSkTelinkCompanyShipParam(SkTelinkCompanyShipParamTestBuilder builder)
        {
            this._SkTelinkCompanyShipParam = builder.Build();
            return this;
        }

        public ShipParticularParamTestBuilder WithReplaceShipNameParam(ReplaceShipNameParamTestBuilder builder)
        {
            this._ReplaceShipNameParam = builder.Build();
            return this;
        }

        public ShipParticularParamTestBuilder WithShipModelTestParam(ShipModelTestParamBuilder builder)
        {
            this._ShipModelTestParam = builder.Build();
            return this;
        }

        public ShipParticularsParam Build()
        {
            return new()
            {
                IsAisToggleOn = _IsAisToggleOn,
                IsGPSToggleOn = _IsGPSToggleOn,
                ShipKey = _ShipKey,
                Callsign = _Callsign,
                ShipName = _ShipName,
                ShipType = _ShipType,
                ShipCode = _ShipCode,
                ShipSatelliteParam = _ShipSatelliteParam,
                SkTelinkCompanyShipParam = _SkTelinkCompanyShipParam,
                ShipModelTestParam = _ShipModelTestParam,
                ReplaceShipNameParam = _ReplaceShipNameParam
            };
        }
    }
}
