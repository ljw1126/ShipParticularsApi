namespace ShipParticularsApi.ValueObjects
{
    public record ReplaceShipNameDetails
    {
        public string ReplacedShipName { get; init; }

        public ReplaceShipNameDetails(string replacedShipName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(replacedShipName);
            ReplacedShipName = replacedShipName;
        }
    }
}
