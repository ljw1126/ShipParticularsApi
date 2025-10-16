namespace ShipParticularsApi.Services.Dtos
{
    public class ShipParticularsParam
    {
        public bool IsAisToggleOn { get; set; } = false;
        public bool IsGPSToggleOn { get; set; } = false;

        public string ShipKey { get; set; }
        public string Callsign { get; set; }
        public string ShipName { get; set; }
        public string ShipType { get; set; }
        public string ShipCode { get; set; }

        public ShipSatelliteParam? ShipSatelliteParam { get; set; }
        public SkTelinkCompanyShipParam? SkTelinkCompanyShipParam { get; set; }
        public ReplaceShipNameParam? ReplaceShipNameParam { get; set; }
        public ShipModelTestParam? ShipModelTestParam { get; set; }
    }
}
