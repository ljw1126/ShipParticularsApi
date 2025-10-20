namespace ShipParticularsApi.Controllers.Dtos.Resps
{
    public class ShipParticularsResp
    {
        public bool IsAisToggleOn { get; set; } = false;
        public bool IsGPSToggleOn { get; set; } = false;

        public string ShipKey { get; set; }
        public string Callsign { get; set; }
        public string ShipName { get; set; }
        public string ShipType { get; set; }
        public string ShipCode { get; set; }

        public ShipSatelliteResp? ShipSatelliteResp { get; set; }
        public SkTelinkCompanyShipResp? SkTelinkCompanyShipResp { get; set; }
        public ReplaceShipNameResp? ReplaceShipNameResp { get; set; }
        public ShipModelTestResp? ShipModelTestResp { get; set; }
    }
}
