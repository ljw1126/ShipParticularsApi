using ShipParticularsApi.Entities;

namespace ShipParticularsApi.Tests.Builders.Entities
{
    public class ShipModelTestBuilder
    {
        private long _Id;
        private string _ShipKey;

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

        public static ShipModelTestBuilder ShipModelTest()
        {
            return new ShipModelTestBuilder();
        }

        public ShipModelTestBuilder WithId(long id)
        {
            _Id = id;
            return this;
        }

        public ShipModelTestBuilder WithShipKey(string shipKey)
        {
            _ShipKey = shipKey;
            return this;
        }

        public ShipModelTestBuilder WithZaBallast(double value)
        {
            _ZaBallast = value;
            return this;
        }

        public ShipModelTestBuilder WithTransverseProjectionAreaBallast(double value)
        {
            _TransverseProjectionAreaBallast = value;
            return this;
        }

        public ShipModelTestBuilder WithTransverseProjectionAreaScantling(double value)
        {
            _TransverseProjectionAreaScantling = value;
            return this;
        }

        public ShipModelTestBuilder WithKyy(double value)
        {
            _Kyy = value;
            return this;
        }

        public ShipModelTestBuilder WithDraftFore(double value)
        {
            _DraftFore = value;
            return this;
        }

        public ShipModelTestBuilder WithDraftAft(double value)
        {
            _DraftAft = value;
            return this;
        }

        public ShipModelTestBuilder WithCbBallast(double value)
        {
            _CbBallast = value;
            return this;
        }

        public ShipModelTestBuilder WithCbScantling(double value)
        {
            _CbScantling = value;
            return this;
        }

        public ShipModelTestBuilder WithSubmergedSurfaceBallast(double value)
        {
            _SubmergedSurfaceBallast = value;
            return this;
        }

        public ShipModelTestBuilder WithSubmergedSurfaceScantling(double value)
        {
            _SubmergedSurfaceScantling = value;
            return this;
        }

        public ShipModelTestBuilder WithMidShipSectionAreaBallast(double value)
        {
            _MidShipSectionAreaBallast = value;
            return this;
        }

        public ShipModelTestBuilder WithMidShipSectionAreaScantling(double value)
        {
            _MidShipSectionAreaScantling = value;
            return this;
        }

        public ShipModelTestBuilder WithDisplacementBallast(double value)
        {
            _DisplacementBallast = value;
            return this;
        }

        public ShipModelTestBuilder WithDisplacementScantling(double value)
        {
            _DisplacementScantling = value;
            return this;
        }

        public ShipModelTestBuilder WithSpeedEtaDBallast(string value)
        {
            _SpeedEtaDBallast = value;
            return this;
        }

        public ShipModelTestBuilder WithEtaDBallast(string value)
        {
            _EtaDBallast = value;
            return this;
        }

        public ShipModelTestBuilder WithSpeedEtaDScantling(string value)
        {
            _SpeedEtaDScantling = value;
            return this;
        }

        public ShipModelTestBuilder WithEtaDScantling(string value)
        {
            _EtaDScantling = value;
            return this;
        }

        public ShipModelTest Build()
        {
            return new()
            {
                Id = _Id,
                ShipKey = _ShipKey,
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
