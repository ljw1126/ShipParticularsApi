using ShipParticularsApi.Controllers.Dtos.Reqs;
using ShipParticularsApi.Services.Dtos.Params;

namespace ShipParticularsApi.Controllers.Dtos.Mappers
{
    public static class ShipParticularsParamMapper
    {
        public static ShipParticularsParam ToParam(ShipParticularsReq req)
        {
            return new()
            {
                IsAisToggleOn = req.IsAisToggleOn,
                IsGPSToggleOn = req.IsGPSToggleOn,
                ShipKey = req.ShipKey,
                Callsign = req.Callsign,
                ShipName = req.ShipName,
                ShipType = req.ShipType,
                ShipCode = req.ShipCode,
                ShipSatelliteParam = ToParam(req.ShipSatelliteReq),
                SkTelinkCompanyShipParam = ToParam(req.SkTelinkCompanyShipReq),
                ReplaceShipNameParam = ToParam(req.ReplaceShipNameReq),
                ShipModelTestParam = ToParam(req.ShipModelTestReq)
            };
        }

        private static ShipSatelliteParam? ToParam(ShipSatelliteReq? shipSatellite)
        {
            if (shipSatellite == null) return null;

            return new()
            {
                SatelliteId = shipSatellite.SatelliteId,
                SatelliteType = shipSatellite.SatelliteType
            };
        }

        private static SkTelinkCompanyShipParam? ToParam(SkTelinkCompanyShipReq? skTelinkCompany)
        {
            if (skTelinkCompany == null) return null;

            return new()
            {
                CompanyName = skTelinkCompany.CompanyName
            };
        }

        private static ReplaceShipNameParam? ToParam(ReplaceShipNameReq? replaceShipName)
        {
            if (replaceShipName == null) return null;

            return new()
            {
                ReplacedShipName = replaceShipName.ReplacedShipName
            };
        }

        private static ShipModelTestParam? ToParam(ShipModelTestReq? shipModel)
        {
            if (shipModel == null) return null;

            return new()
            {
                ZaBallast = shipModel.ZaBallast,
                TransverseProjectionAreaBallast = shipModel.TransverseProjectionAreaBallast,
                TransverseProjectionAreaScantling = shipModel.TransverseProjectionAreaScantling,
                Kyy = shipModel.Kyy,
                DraftFore = shipModel.DraftFore,
                DraftAft = shipModel.DraftAft,
                CbBallast = shipModel.CbBallast,
                CbScantling = shipModel.CbScantling,
                SubmergedSurfaceBallast = shipModel.SubmergedSurfaceBallast,
                SubmergedSurfaceScantling = shipModel.SubmergedSurfaceScantling,
                MidShipSectionAreaBallast = shipModel.MidShipSectionAreaBallast,
                MidShipSectionAreaScantling = shipModel.MidShipSectionAreaScantling,
                DisplacementBallast = shipModel.DisplacementBallast,
                DisplacementScantling = shipModel.DisplacementScantling,
                SpeedEtaDBallast = shipModel.SpeedEtaDBallast,
                EtaDBallast = shipModel.EtaDBallast,
                SpeedEtaDScantling = shipModel.SpeedEtaDScantling,
                EtaDScantling = shipModel.EtaDScantling
            };
        }
    }
}
