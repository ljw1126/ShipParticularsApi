using ShipParticularsApi.Exceptions;

namespace ShipParticularsApi.Entities.Enums
{
    public static class ShipTypesConverter
    {
        public static ShipTypes ToEnum(string value)
        {
            return value.ToUpperInvariant() switch
            {
                "-" => ShipTypes.Default,
                "RORO" => ShipTypes.Roro,
                "ROPAX" => ShipTypes.Ropax,
                "CRUISE_PASSENGER" => ShipTypes.CruisePassenger,
                "FISHING" => ShipTypes.Fishing,
                "REFRIGERATED_CARGO" => ShipTypes.RefrigeratedCargo,
                "GENERAL_CARGO" => ShipTypes.GeneralCargo,
                "PASSENGER" => ShipTypes.Passenger,
                "VEHICLE" => ShipTypes.Vehicle,
                "LNG_CARRIER" => ShipTypes.LngCarrier,
                "BULK_CARRIER" => ShipTypes.BulkCarrier,
                "COMBINATION" => ShipTypes.Combination,
                "CHEMICAL" => ShipTypes.Chemical,
                "CONTAINER" => ShipTypes.Container,
                "SPECIAL_CRAFT" => ShipTypes.SpecialCraft,
                "Cargo" => ShipTypes.Cargo,
                "OTHER" => ShipTypes.Other,
                "GAS_CARRIER" => ShipTypes.GasCarrier,
                "OIL_TANKER" => ShipTypes.OilTanker,
                "Tanker" => ShipTypes.Tanker,
                _ => throw new InvalidOperationException($"선박 타입 변환 실패: 허용되지 않은 문자열 값 '{value}'")
            };
        }

        public static string ToString(ShipTypes types)
        {
            return types switch
            {
                ShipTypes.Default => "-",
                ShipTypes.Roro => "RORO",
                ShipTypes.Ropax => "ROPAX",
                ShipTypes.CruisePassenger => "CRUISE_PASSENGER",
                ShipTypes.Fishing => "FISHING",
                ShipTypes.RefrigeratedCargo => "REFRIGERATED_CARGO",
                ShipTypes.GeneralCargo => "GENERAL_CARGO",
                ShipTypes.Passenger => "PASSENGER",
                ShipTypes.Vehicle => "VEHICLE",
                ShipTypes.LngCarrier => "LNG_CARRIER",
                ShipTypes.BulkCarrier => "BULK_CARRIER",
                ShipTypes.Combination => "COMBINATION",
                ShipTypes.Chemical => "CHEMICAL",
                ShipTypes.Container => "CONTAINER",
                ShipTypes.SpecialCraft => "SPECIAL_CRAFT",
                ShipTypes.Cargo => "Cargo",
                ShipTypes.Other => "OTHER",
                ShipTypes.GasCarrier => "GAS_CARRIER",
                ShipTypes.OilTanker => "OIL_TANKER",
                ShipTypes.Tanker => "Tanker",
                _ => throw new InvalidOperationException($"유효하지 않은 선박 타입 값 입니다. '{types}'")
            };
        }

        public static ShipTypes ParseFromRequest(string value)
        {
            try
            {
                return ToEnum(value);
            }
            catch (InvalidOperationException)
            {
                throw new BadRequestException($"유효하지 않은 선박 타입 입니다. 입력 값: '{value}'");
            }
        }
    }
}
