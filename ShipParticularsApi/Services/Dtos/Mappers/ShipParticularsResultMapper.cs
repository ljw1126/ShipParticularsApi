using ShipParticularsApi.Entities;
using ShipParticularsApi.Entities.Enums;
using ShipParticularsApi.Services.Dtos.Results;

namespace ShipParticularsApi.Services.Dtos.Mapper
{
    public static class ShipParticularsResultMapper
    {
        public static ShipParticularsResult ToResult(ShipInfo shipInfo)
        {
            return new()
            {
                IsAisToggleOn = shipInfo.HasSatAisService(),
                IsGPSToggleOn = shipInfo.HasKtSatService(),
                ShipKey = shipInfo.ShipKey,
                Callsign = shipInfo.Callsign,
                ShipName = shipInfo.ShipName,
                ShipType = shipInfo.ShipType.HasValue ? ShipTypesConverter.ToString(shipInfo.ShipType.Value) : null,
                ShipCode = shipInfo.ShipCode,
                ReplaceShipNameResult = ToResult(shipInfo.ReplaceShipName),
                ShipModelTestResult = ToResult(shipInfo.ShipModelTest),
                ShipSatelliteResult = ToResult(shipInfo.ShipSatellite),
                SkTelinkCompanyShipResult = ToResult(shipInfo.SkTelinkCompanyShip)
            };
        }

        private static ReplaceShipNameResult? ToResult(ReplaceShipName? replaceShipName)
        {
            if (replaceShipName == null) return null;

            return new()
            {
                ReplacedShipName = replaceShipName.ReplacedShipName
            };
        }

        private static ShipModelTestResult? ToResult(ShipModelTest? shipModelTest)
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

        private static ShipSatelliteResult? ToResult(ShipSatellite? shipSatellite)
        {
            if (shipSatellite == null) return null;

            return new()
            {
                SatelliteId = shipSatellite.SatelliteId,
                SatelliteType = shipSatellite.SatelliteType.HasValue
                    ? SatelliteTypesConverter.ToString(shipSatellite.SatelliteType.Value)
                    : null
            };
        }

        private static SkTelinkCompanyShipResult ToResult(SkTelinkCompanyShip? skTelinkCompanyShip)
        {
            if (skTelinkCompanyShip == null) return null;

            return new()
            {
                CompanyName = skTelinkCompanyShip.CompanyName
            };
        }
    }
}
