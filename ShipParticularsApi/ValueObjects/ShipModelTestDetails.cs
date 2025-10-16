using ShipParticularsApi.Services.Dtos;

namespace ShipParticularsApi.ValueObjects
{
    public record ShipModelTestDetails(
        double ZaBallast,
        double TransverseProjectionAreaBallast,
        double TransverseProjectionAreaScantling,
        double Kyy,
        double DraftFore,
        double DraftAft,
        double CbBallast,
        double CbScantling,
        double SubmergedSurfaceBallast,
        double SubmergedSurfaceScantling,
        double MidShipSectionAreaBallast,
        double MidShipSectionAreaScantling,
        double DisplacementBallast,
        double DisplacementScantling,
        string SpeedEtaDBallast,
        string EtaDBallast,
        string SpeedEtaDScantling,
        string EtaDScantling
    )
    {
        public static ShipModelTestDetails From(ShipModelTestParam param)
        {
            return new ShipModelTestDetails(
                param.ZaBallast,
                param.TransverseProjectionAreaBallast,
                param.TransverseProjectionAreaScantling,
                param.Kyy,
                param.DraftFore,
                param.DraftAft,
                param.CbBallast,
                param.CbScantling,
                param.SubmergedSurfaceBallast,
                param.SubmergedSurfaceScantling,
                param.MidShipSectionAreaBallast,
                param.MidShipSectionAreaScantling,
                param.DisplacementBallast,
                param.DisplacementScantling,
                param.SpeedEtaDBallast,
                param.EtaDBallast,
                param.SpeedEtaDScantling,
                param.EtaDScantling
            );
        }
    }
}
