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
        public long Id { get; private set; }

        [Required]
        [Column("SHIP_KEY")]
        [MaxLength(10)]
        public string ShipKey { get; private set; }

        [Column("CALLSIGN")]
        [MaxLength(10)]
        public string Callsign { get; private set; }

        [Column("SHIP_NAME")]
        [MaxLength(10)]
        public string ShipName { get; private set; }

        [Column("SHIP_TYPE")]
        [MaxLength(100)]
        public ShipTypes? ShipType { get; private set; }

        [Column("SHIP_CODE")]
        [MaxLength(100)]
        public string? ShipCode { get; private set; }

        [Column("EXTERNAL_SHIP_ID")]
        [MaxLength(36)]
        public string? ExternalShipId { get; private set; }

        [Column("IS_USE_KTSAT")]
        public bool? IsUseKtsat { get; private set; }

        [Column("IS_SERVICE")]
        public bool? IsService { get; private set; }

        [Column("IS_USE_AIS")]
        public bool IsUseAis { get; private set; }

        public virtual ReplaceShipName? ReplaceShipName { get; private set; }
        public virtual ShipModelTest? ShipModelTest { get; private set; }
        public virtual ShipSatellite? ShipSatellite { get; private set; }
        public virtual ICollection<ShipService>? ShipServices { get; private set; } = [];
        public virtual SkTelinkCompanyShip? SkTelinkCompanyShip { get; private set; }

        public ShipInfo(
            string shipKey,
            string callsign,
            string shipName,
            ShipTypes? shipType,
            string? shipCode,
            bool? isService
        ) : this(
            0L,
            shipKey,
            callsign,
            shipName,
            shipType,
            shipCode,
            null,
            false,
            isService,
            false,
            null,
            null,
            null,
            [],
            null)
        {
        }

        public ShipInfo(
            long id,
            string shipKey,
            string callsign,
            string shipName,
            ShipTypes? shipType,
            string? shipCode,
            string? externalShipId,
            bool? isUseKtsat,
            bool? isService,
            bool isUseAis,
            ReplaceShipName? replaceShipName,
            ShipModelTest? shipModelTest,
            ShipSatellite? shipSatellite,
            ICollection<ShipService>? shipServices,
            SkTelinkCompanyShip? skTelinkCompanyShip
        )
        {
            Id = id;
            ShipKey = shipKey;
            Callsign = callsign;
            ShipName = shipName;
            ShipType = shipType;
            ShipCode = shipCode;
            ExternalShipId = externalShipId;
            IsUseKtsat = isUseKtsat;
            IsService = isService;
            IsUseAis = isUseAis;
            ReplaceShipName = replaceShipName;
            ShipModelTest = shipModelTest;
            ShipSatellite = shipSatellite;
            ShipServices = shipServices;
            SkTelinkCompanyShip = skTelinkCompanyShip;
        }

        public static ShipInfo From(ShipInfoDetails details)
        {
            ArgumentException.ThrowIfNullOrEmpty(details.ShipKey);
            ArgumentException.ThrowIfNullOrEmpty(details.Callsign);
            ArgumentException.ThrowIfNullOrEmpty(details.ShipName);

            return new(
                details.ShipKey,
                details.Callsign,
                details.ShipName,
                ShipTypesConverter.ParseFromRequest(details.ShipType),
                details.ShipCode,
                true
            );
        }

        public ShipInfo UpdateDetails(ShipInfoDetails details)
        {
            this.Callsign = details.Callsign;
            this.ShipName = details.ShipName;
            this.ShipType = ShipTypesConverter.ParseFromRequest(details.ShipType);
            this.ShipCode = details.ShipCode;
            return this;
        }

        public void ActiveAisService()
        {
            if (this.HasSatAisService()) return;

            this.ShipServices.Add(ShipService.Of(this.ShipKey, ServiceNameTypes.SatAis));
            this.ActiveAis();
        }

        public void DeactiveAisService()
        {
            if (!this.HasSatAisService()) return;

            var existingService = this.ShipServices.First(s => s.ServiceName == ServiceNameTypes.SatAis);
            this.ShipServices.Remove(existingService);
            this.DeactiveAis();
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

        public void ActiveGpsService(SatelliteDetails details, string userId)
        {
            this.ManageGpsService(details.SatelliteId, details.SatelliteType, userId);
            this.ManageSkTelinkCompanyShip(details.CompanyName);
        }

        private void ManageGpsService(string? satelliteId, string? satelliteType, string userId)
        {
            if (HasKtSatService())
            {
                this.ShipSatellite.Update(satelliteId, satelliteType, userId);
            }
            else
            {
                this.ShipServices.Add(ShipService.Of(this.ShipKey, ServiceNameTypes.KtSat));
                this.ShipSatellite = ShipSatellite.Of(this.ShipKey, satelliteId, satelliteType, userId);
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

        public void DeactiveGpsService()
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
