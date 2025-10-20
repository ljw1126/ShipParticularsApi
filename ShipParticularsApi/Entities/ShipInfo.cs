using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ShipParticularsApi.Entities.Enums;
using ShipParticularsApi.ValueObjects;

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
        public bool IsUseAis { get; set; }

        public virtual ReplaceShipName? ReplaceShipName { get; set; }
        public virtual ShipModelTest? ShipModelTest { get; set; }
        public virtual ShipSatellite? ShipSatellite { get; set; }
        public virtual ICollection<ShipService>? ShipServices { get; set; } = [];
        public virtual SkTelinkCompanyShip? SkTelinkCompanyShip { get; set; }

        public static ShipInfo From(ShipInfoDetails details)
        {
            ArgumentException.ThrowIfNullOrEmpty(details.ShipKey);
            ArgumentException.ThrowIfNullOrEmpty(details.Callsign);
            ArgumentException.ThrowIfNullOrEmpty(details.ShipName);

            return new()
            {
                ShipKey = details.ShipKey,
                Callsign = details.Callsign,
                ShipName = details.ShipName,
                ShipType = ShipTypesConverter.ToShipTypes(details.ShipType),
                ShipCode = details.ShipCode
            };
        }

        public ShipInfo UpdateDetails(ShipInfoDetails details)
        {
            this.Callsign = details.Callsign;
            this.ShipName = details.ShipName;
            this.ShipType = ShipTypesConverter.ToShipTypes(details.ShipType);
            this.ShipCode = details.ShipCode;
            return this;
        }

        public void ManageAisService(bool isAisToggleOn)
        {
            if (ShouldActivateAis(isAisToggleOn))
            {
                this.ShipServices.Add(ShipService.Of(this.ShipKey, ServiceNameTypes.SatAis));
                this.ActiveAis();
                return;
            }

            if (ShouldDeactivateAis(isAisToggleOn))
            {
                var existingService = this.ShipServices.First(s => s.ServiceName == ServiceNameTypes.SatAis);
                this.ShipServices.Remove(existingService);
                this.DeactiveAis();
            }
        }

        private bool ShouldActivateAis(bool isAisToggleOn)
        {
            return isAisToggleOn && !this.HasSatAisService();
        }

        private bool ShouldDeactivateAis(bool isAisToggleOn)
        {
            return !isAisToggleOn && this.HasSatAisService();
        }

        public bool HasSatAisService()
        {
            return this.ShipServices.Any(s => s.ServiceName == ServiceNameTypes.SatAis);
        }

        private void ActiveAis()
        {
            this.IsUseAis = true;
        }

        private void DeactiveAis()
        {
            this.IsUseAis = false;
        }

        public void ManageGpsService(bool isGPSToggleOn, SatelliteDetails details)
        {
            if (isGPSToggleOn)
            {
                this.ActivateGpsService(details.SatelliteId, details.SatelliteType);
                this.ManageSkTelinkCompanyShip(details.CompanyName);
            }
            else
            {
                DeactiveGpsService();
            }
        }

        private void ActivateGpsService(string? satelliteId, string? satelliteType)
        {
            if (HasKtSatService())
            {
                this.ShipSatellite.Update(satelliteId, satelliteType);
            }
            else
            {
                this.ShipServices.Add(ShipService.Of(this.ShipKey, ServiceNameTypes.KtSat));
                this.ShipSatellite = ShipSatellite.Of(this.ShipKey, satelliteId, satelliteType);
            }

            this.ExternalShipId = satelliteId;
            this.IsUseKtsat = true;
        }

        private void ManageSkTelinkCompanyShip(string? companyName)
        {
            if (this.ShipSatellite == null)
            {
                return;
            }

            if (!this.ShipSatellite.IsSkTelink())
            {
                this.SkTelinkCompanyShip = null;
                return;
            }

            if (this.SkTelinkCompanyShip == null)
            {
                this.SkTelinkCompanyShip = SkTelinkCompanyShip.Of(this.ShipKey, companyName);
            }
            else
            {
                this.SkTelinkCompanyShip.Update(companyName);
            }
        }

        private void DeactiveGpsService()
        {
            if (!HasKtSatService())
            {
                return;
            }

            if (this.ShipSatellite != null && this.ShipSatellite.IsSkTelink())
            {
                this.SkTelinkCompanyShip = null;
            }

            this.ShipSatellite = null;
            this.ExternalShipId = null;
            this.IsUseKtsat = false;

            var existingService = this.ShipServices.First(s => s.ServiceName == ServiceNameTypes.KtSat);
            this.ShipServices.Remove(existingService);
        }

        public bool HasKtSatService()
        {
            return this.ShipServices.Any(s => s.ServiceName == ServiceNameTypes.KtSat);
        }

        public void ManageShipModelTest(ShipModelTestDetails data)
        {
            if (this.ShipModelTest == null)
            {
                this.ShipModelTest = ShipModelTest.From(this.ShipKey, data);
            }
            else
            {
                this.ShipModelTest.Update(data);
            }
        }

        public void ManageReplaceShipName(ReplaceShipNameDetails data)
        {
            if (this.ReplaceShipName == null)
            {
                this.ReplaceShipName = ReplaceShipName.From(this.ShipKey, data);
            }
            else
            {
                this.ReplaceShipName.Update(data);
            }
        }
    }
}
