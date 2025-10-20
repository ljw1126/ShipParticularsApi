namespace ShipParticularsApi.Services.Dtos.Params
{
    public class ShipModelTestParam
    {
        public double ZaBallast { get; set; }

        public double TransverseProjectionAreaBallast { get; set; }

        public double TransverseProjectionAreaScantling { get; set; }

        public double Kyy { get; set; }

        public double DraftFore { get; set; }

        public double DraftAft { get; set; }

        public double CbBallast { get; set; }

        public double CbScantling { get; set; }

        public double SubmergedSurfaceBallast { get; set; }

        public double SubmergedSurfaceScantling { get; set; }

        public double MidShipSectionAreaBallast { get; set; }

        public double MidShipSectionAreaScantling { get; set; }

        public double DisplacementBallast { get; set; }

        public double DisplacementScantling { get; set; }

        public string SpeedEtaDBallast { get; set; }

        public string EtaDBallast { get; set; }

        public string SpeedEtaDScantling { get; set; }

        public string EtaDScantling { get; set; }
    }
}
