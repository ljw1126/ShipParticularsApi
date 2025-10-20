using ShipParticularsApi.Services.Dtos.Params;

namespace ShipParticularsApi.ValueObjects
{
    public record ShipInfoDetails(
        string ShipKey,
        string Callsign,
        string ShipName,
        string ShipType,
        string ShipCode)
    {
        public static ShipInfoDetails From(ShipParticularsParam param)
        {
            return new ShipInfoDetails(
                param.ShipKey,
                param.Callsign,
                param.ShipName,
                param.ShipType,
                param.ShipCode
            );
        }
    }
}
