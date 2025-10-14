using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShipParticularsApi.Entities
{
    [Table("SHIP_SATELLITE")]
    public class ShipSatellite
    {
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [Column("SHIP_KEY")]
        [MaxLength(10)]
        public string ShipKey { get; set; }

        public virtual ShipInfo ShipInfo { get; set; }

        [Column("SATELLITE_TYPE")]
        [MaxLength(200)]
        public SatelliteTypes? SatelliteType { get; set; }

        [Column("SATELLITE_ID")]
        [MaxLength(256)]
        public string? SatelliteId { get; set; }

        [Column("IS_USE_SATELLITE")]
        public bool IsUseSatellite { get; set; }

        [Column("CREATE_USER_ID")]
        [MaxLength(200)]
        public string? CreateUserId { get; set; }

        [Column("CREATE_DATE_TIME")]
        public DateTime CreateDateTime { get; set; }

        [Column("UPDATE_USER_ID")]
        [MaxLength(200)]
        public string? UpdateUserId { get; set; }

        [Column("UPDATE_DATE_TIME")]
        public DateTime? UpdateDateTime { get; set; }

        public static ShipSatellite Of(string shipKey, string satelliteId, string satelliteType)
        {
            return new()
            {
                ShipKey = shipKey,
                SatelliteId = satelliteId,
                SatelliteType = ConvertStringToSatelliteType(satelliteType),
                IsUseSatellite = true,
                UpdateUserId = null,
                UpdateDateTime = null
            };
        }

        private static SatelliteTypes ConvertStringToSatelliteType(string value)
        {
            return value switch
            {
                "NONE" => SatelliteTypes.None,
                "KT_SAT" => SatelliteTypes.KtSat,
                "SK_TELINK" => SatelliteTypes.SkTelink,
                "SYNER_SAT" => SatelliteTypes.SynerSat,
                _ => throw new ArgumentException($"Invalid string value '{value}' for SatelliteTypes enum")
            };
        }

        public bool IsSkTelink()
        {
            return SatelliteType == SatelliteTypes.SkTelink;
        }
    }
}
