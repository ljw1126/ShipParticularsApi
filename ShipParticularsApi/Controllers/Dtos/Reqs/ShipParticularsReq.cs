namespace ShipParticularsApi.Controllers.Dtos.Reqs
{
    public class ShipParticularsReq
    {
        public bool IsAisToggleOn { get; set; } = false;
        public bool IsGPSToggleOn { get; set; } = false;

        public string ShipKey { get; set; }
        public string Callsign { get; set; }
        public string ShipName { get; set; }
        public string ShipType { get; set; }
        public string ShipCode { get; set; }

        public ShipSatelliteReq? ShipSatelliteReq { get; set; }
        public SkTelinkCompanyShipReq? SkTelinkCompanyShipReq { get; set; }
        public ReplaceShipNameReq? ReplaceShipNameReq { get; set; }
        public ShipModelTestReq? ShipModelTestReq { get; set; }
    }
}
