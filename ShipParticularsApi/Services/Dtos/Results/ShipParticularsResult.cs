namespace ShipParticularsApi.Services.Dtos.Results
{
    public class ShipParticularsResult
    {
        public bool IsAisToggleOn { get; set; } = false;
        public bool IsGPSToggleOn { get; set; } = false;

        public string ShipKey { get; set; }
        public string Callsign { get; set; }
        public string ShipName { get; set; }
        public string ShipType { get; set; }
        public string ShipCode { get; set; }

        public ShipSatelliteResult? ShipSatelliteResult { get; set; }
        public SkTelinkCompanyShipResult? SkTelinkCompanyShipResult { get; set; }
        public ReplaceShipNameResult? ReplaceShipNameResult { get; set; }
        public ShipModelTestResult? ShipModelTestResult { get; set; }
    }
}
