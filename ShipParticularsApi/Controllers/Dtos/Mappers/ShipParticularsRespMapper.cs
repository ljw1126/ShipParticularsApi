using ShipParticularsApi.Controllers.Dtos.Resps;
using ShipParticularsApi.Services.Dtos.Results;

namespace ShipParticularsApi.Controllers.Dtos.Mappers
{
    public static class ShipParticularsRespMapper
    {
        public static ShipParticularsResp ToResp(ShipParticularsResult result)
        {
            return new()
            {
                IsAisToggleOn = result.IsAisToggleOn,
                IsGPSToggleOn = result.IsGPSToggleOn,
                ShipKey = result.ShipKey,
                Callsign = result.Callsign,
                ShipName = result.ShipName,
                ShipType = result.ShipType,
                ShipCode = result.ShipCode,
                ReplaceShipNameResp = ToResp(result.ReplaceShipNameResult),
                ShipModelTestResp = ToResp(result.ShipModelTestResult),
                ShipSatelliteResp = ToResp(result.ShipSatelliteResult),
                SkTelinkCompanyShipResp = ToResp(result.SkTelinkCompanyShipResult)
            };
        }

        private static ReplaceShipNameResp? ToResp(ReplaceShipNameResult? replaceShipName)
        {
            if (replaceShipName == null) return null;

            return new()
            {
                ReplacedShipName = replaceShipName.ReplacedShipName
            };
        }

        private static ShipModelTestResp? ToResp(ShipModelTestResult? shipModelTest)
        {
            if (shipModelTest == null) return null;

            return new()
            {

                ZaBallast = shipModelTest.ZaBallast,
                TransverseProjectionAreaBallast = shipModelTest.TransverseProjectionAreaBallast,
                TransverseProjectionAreaScantling = shipModelTest.TransverseProjectionAreaScantling,
                Kyy = shipModelTest.Kyy,
                DraftFore = shipModelTest.DraftFore,
                DraftAft = shipModelTest.DraftAft,
                CbBallast = shipModelTest.CbBallast,
                CbScantling = shipModelTest.CbScantling,
                SubmergedSurfaceBallast = shipModelTest.SubmergedSurfaceBallast,
                SubmergedSurfaceScantling = shipModelTest.SubmergedSurfaceScantling,
                MidShipSectionAreaBallast = shipModelTest.MidShipSectionAreaBallast,
                MidShipSectionAreaScantling = shipModelTest.MidShipSectionAreaScantling,
                DisplacementBallast = shipModelTest.DisplacementBallast,
                DisplacementScantling = shipModelTest.DisplacementScantling,
                SpeedEtaDBallast = shipModelTest.SpeedEtaDBallast,
                EtaDBallast = shipModelTest.EtaDBallast,
                SpeedEtaDScantling = shipModelTest.SpeedEtaDScantling,
                EtaDScantling = shipModelTest.EtaDScantling
            };
        }

        private static ShipSatelliteResp? ToResp(ShipSatelliteResult? shipSatellite)
        {
            if (shipSatellite == null) return null;

            return new()
            {
                SatelliteId = shipSatellite.SatelliteId,
                SatelliteType = shipSatellite.SatelliteType
            };
        }

        private static SkTelinkCompanyShipResp? ToResp(SkTelinkCompanyShipResult? skTelinkCompanyShip)
        {
            if (skTelinkCompanyShip == null) return null;

            return new()
            {
                CompanyName = skTelinkCompanyShip.CompanyName
            };
        }
    }
}
