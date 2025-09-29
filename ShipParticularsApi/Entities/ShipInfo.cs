using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

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
        public string ShipType { get; set; }

        [Column("SHIP_CODE")]
        [MaxLength(100)]
        public string ShipCode { get; set; }

        [Column("EXTERNAL_SHIP_ID")]
        [MaxLength(36)]
        public string ExternalShipId { get; set; }

        [Column("IS_USE_KTSAT")]
        public bool IsUseKtsat { get; set; }

        [Column("IS_SERVICE")]
        public bool IsService { get; set; }

        [Column("IS_USE_AIS")]
        public bool IsUseAis { get; set; }

        public virtual ICollection<ReplaceShipName>? ReplaceShipNames { get; set; }
        public virtual ICollection<ShipModelTest>? ShipModelTests { get; set; }
        public virtual ICollection<ShipSatellite>? ShipSatellites { get; set; }
        public virtual ICollection<ShipService>? ShipServices { get; set; }
        public virtual ICollection<SkTelinkCompanyShip>? SkTelinkCompanyShips { get; set; }
    }
}
