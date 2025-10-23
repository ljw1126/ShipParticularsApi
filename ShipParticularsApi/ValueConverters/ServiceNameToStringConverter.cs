using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShipParticularsApi.Entities;
using ShipParticularsApi.Entities.Enums;

namespace ShipParticularsApi.ValueConverters
{
    public class ServiceNameToStringConverter : ValueConverter<ServiceNameTypes, string>
    {
        public ServiceNameToStringConverter() : base(
            v => ServiceNameTypesConverter.ToString(v),
            v => ServiceNameTypesConverter.ToEnum(v))
        {
        }
    }
}
