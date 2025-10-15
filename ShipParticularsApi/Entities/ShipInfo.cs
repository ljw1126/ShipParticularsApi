using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ShipParticularsApi.Tests.Services;

namespace ShipParticularsApi.Entities
{
    [Table("SHIP_INFO")]
    [Index(nameof(ShipKey), IsUnique = true)]
    public class ShipInfo
    {
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [Column("SHIP_KEY")]
        [MaxLength(10)]
        public string ShipKey { get; set; }

        [Column("CALLSIGN")]
        [MaxLength(10)]
        public string Callsign { get; set; }

        [Column("SHIP_NAME")]
        [MaxLength(10)]
        public string ShipName { get; set; }

        [Column("SHIP_TYPE")]
        [MaxLength(100)]
        public ShipTypes? ShipType { get; set; }

        [Column("SHIP_CODE")]
        [MaxLength(100)]
        public string? ShipCode { get; set; }

        [Column("EXTERNAL_SHIP_ID")]
        [MaxLength(36)]
        public string? ExternalShipId { get; set; }

        [Column("IS_USE_KTSAT")]
        public bool? IsUseKtsat { get; set; }

        [Column("IS_SERVICE")]
        public bool? IsService { get; set; }

        [Column("IS_USE_AIS")]
        public bool? IsUseAis { get; set; }

        public virtual ReplaceShipName? ReplaceShipName { get; set; }
        public virtual ShipModelTest? ShipModelTest { get; set; }
        public virtual ShipSatellite? ShipSatellite { get; set; }
        public virtual ICollection<ShipService>? ShipServices { get; set; } = [];
        public virtual SkTelinkCompanyShip? SkTelinkCompanyShip { get; set; }

        public static ShipInfo From(ShipParticularsServiceTests.ShipParticularsParam param)
        {
            return new()
            {
                ShipKey = param.ShipKey,
                Callsign = param.Callsign,
                ShipName = param.ShipName,
                ShipType = ConvertStringToShipTypes(param.ShipType),
                ShipCode = param.ShipCode
            };
        }

        private static ShipTypes ConvertStringToShipTypes(string value)
        {
            return value switch
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
                _ => throw new ArgumentException($"Invalid ShipType : {value}")
            };
        }

        public ShipInfo Update(ShipParticularsServiceTests.ShipParticularsParam param)
        {
            ShipKey = param.ShipKey;
            Callsign = param.Callsign;
            ShipName = param.ShipName;
            ShipType = ConvertStringToShipTypes(param.ShipType);
            ShipCode = param.ShipCode;
            return this;
        }

        public void EnableAis()
        {
            IsUseAis = true;
        }

        public void DisableAis()
        {
            IsUseAis = false;
        }

        public void ManageAisService(bool isAisToggleOn)
        {
            var existingService = this.ShipServices.FirstOrDefault(s => s.ServiceName == ServiceNameTypes.SatAis);

            if (isAisToggleOn)
            {
                if (existingService == null)
                {
                    this.ShipServices.Add(ShipService.Of(this.ShipKey, ServiceNameTypes.SatAis));
                }

                this.IsUseAis = true;
            }
            else
            {
                if (existingService != null)
                {
                    this.ShipServices.Remove(existingService);
                }

                this.IsUseAis = false;
            }
        }

        public void ManageGpsService(bool isGPSToggleOn, string? satelliteId, string? satelliteType, string? companyName)
        {
            var existingService = this.ShipServices.FirstOrDefault(s => s.ServiceName == ServiceNameTypes.KtSat);

            if (isGPSToggleOn)
            {
                if (existingService == null)
                {
                    this.ShipServices.Add(ShipService.Of(this.ShipKey, ServiceNameTypes.KtSat)); // NOTE. 무조건 ServiceNameTypes.KtSat ?

                    this.ShipSatellite = ShipSatellite.Of(this.ShipKey, satelliteId, satelliteType);
                    this.ExternalShipId = satelliteId;
                    this.IsUseKtsat = true; // NOTE. SHIP_SATELLITE.IS_USE_SATELLITE (bit)와 동일?
                }

                if (this.ShipSatellite != null && this.ShipSatellite.IsSkTelink())
                {
                    // 신규 ShipInfo이고 SK Telink 위성을 사용하는 경우 SkTelinkCompanyShip을 추가해야 한다
                    this.SkTelinkCompanyShip = SkTelinkCompanyShip.Of(this.ShipKey, companyName);
                }
            }
            else
            {
                if (existingService != null)
                {
                    this.ShipServices.Remove(existingService);

                    this.SkTelinkCompanyShip = null;

                    this.ShipSatellite = null;
                    this.ExternalShipId = null;
                    this.IsUseKtsat = false;
                }
            }
        }
    }
}
