using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShipParticularsApi.Entities.Enums;

namespace ShipParticularsApi.ValueConverters
{
    public class ShipTypesToStringConverter : ValueConverter<ShipTypes, string>
    {
        public ShipTypesToStringConverter() : base(
            v => ShipTypesConverter.ToString(v),
            v => ShipTypesConverter.ToShipTypes(v)
        )
        {
        }
    }
}
