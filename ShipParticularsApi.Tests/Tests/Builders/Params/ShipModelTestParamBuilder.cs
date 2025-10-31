using ShipParticularsApi.Services.Dtos.Params;

namespace ShipParticularsApi.Tests.Tests.Builders.Params
{
    public class ShipModelTestParamBuilder
    {
        private double _ZaBallast;

        private double _TransverseProjectionAreaBallast;

        private double _TransverseProjectionAreaScantling;

        private double _Kyy;

        private double _DraftFore;

        private double _DraftAft;

        private double _CbBallast;

        private double _CbScantling;

        private double _SubmergedSurfaceBallast;

        private double _SubmergedSurfaceScantling;

        private double _MidShipSectionAreaBallast;

        private double _MidShipSectionAreaScantling;

        private double _DisplacementBallast;

        private double _DisplacementScantling;

        private string _SpeedEtaDBallast;

        private string _EtaDBallast;

        private string _SpeedEtaDScantling;

        private string _EtaDScantling;

        public static ShipModelTestParamBuilder ShipModelTestParam()
        {
            return new ShipModelTestParamBuilder();
        }

        public ShipModelTestParamBuilder WithZaBallast(double value)
        {
            _ZaBallast = value;
            return this;
        }

        public ShipModelTestParamBuilder WithTransverseProjectionAreaBallast(double value)
        {
            _TransverseProjectionAreaBallast = value;
            return this;
        }

        public ShipModelTestParamBuilder WithTransverseProjectionAreaScantling(double value)
        {
            _TransverseProjectionAreaScantling = value;
            return this;
        }

        public ShipModelTestParamBuilder WithKyy(double value)
        {
            _Kyy = value;
            return this;
        }

        public ShipModelTestParamBuilder WithDraftFore(double value)
        {
            _DraftFore = value;
            return this;
        }

        public ShipModelTestParamBuilder WithDraftAft(double value)
        {
            _DraftAft = value;
            return this;
        }

        public ShipModelTestParamBuilder WithCbBallast(double value)
        {
            _CbBallast = value;
            return this;
        }

        public ShipModelTestParamBuilder WithCbScantling(double value)
        {
            _CbScantling = value;
            return this;
        }

        public ShipModelTestParamBuilder WithSubmergedSurfaceBallast(double value)
        {
            _SubmergedSurfaceBallast = value;
            return this;
        }

        public ShipModelTestParamBuilder WithSubmergedSurfaceScantling(double value)
        {
            _SubmergedSurfaceScantling = value;
            return this;
        }

        public ShipModelTestParamBuilder WithMidShipSectionAreaBallast(double value)
        {
            _MidShipSectionAreaBallast = value;
            return this;
        }

        public ShipModelTestParamBuilder WithMidShipSectionAreaScantling(double value)
        {
            _MidShipSectionAreaScantling = value;
            return this;
        }

        public ShipModelTestParamBuilder WithDisplacementBallast(double value)
        {
            _DisplacementBallast = value;
            return this;
        }

        public ShipModelTestParamBuilder WithDisplacementScantling(double value)
        {
            _DisplacementScantling = value;
            return this;
        }

        public ShipModelTestParamBuilder WithSpeedEtaDBallast(string value)
        {
            _SpeedEtaDBallast = value;
            return this;
        }

        public ShipModelTestParamBuilder WithEtaDBallast(string value)
        {
            _EtaDBallast = value;
            return this;
        }

        public ShipModelTestParamBuilder WithSpeedEtaDScantling(string value)
        {
            _SpeedEtaDScantling = value;
            return this;
        }

        public ShipModelTestParamBuilder WithEtaDScantling(string value)
        {
            _EtaDScantling = value;
            return this;
        }

        public ShipModelTestParam Build()
        {
            return new()
            {
                ZaBallast = _ZaBallast,
                TransverseProjectionAreaBallast = _TransverseProjectionAreaBallast,
                TransverseProjectionAreaScantling = _TransverseProjectionAreaScantling,
                Kyy = _Kyy,
                DraftFore = _DraftFore,
                DraftAft = _DraftAft,
                CbBallast = _CbBallast,
                CbScantling = _CbScantling,
                SubmergedSurfaceBallast = _SubmergedSurfaceBallast,
                SubmergedSurfaceScantling = _SubmergedSurfaceScantling,
                MidShipSectionAreaBallast = _MidShipSectionAreaBallast,
                MidShipSectionAreaScantling = _MidShipSectionAreaScantling,
                DisplacementBallast = _DisplacementBallast,
                DisplacementScantling = _DisplacementScantling,
                SpeedEtaDBallast = _SpeedEtaDBallast,
                EtaDBallast = _EtaDBallast,
                SpeedEtaDScantling = _SpeedEtaDScantling,
                EtaDScantling = _EtaDScantling
            };
        }
    }
}
