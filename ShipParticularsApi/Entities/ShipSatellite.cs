using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShipParticularsApi.Entities.Enums;

namespace ShipParticularsApi.Entities
{
    [Table("SHIP_SATELLITE")]
    public class ShipSatellite : BaseEntity
    {
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [Column("SHIP_KEY")]
        [MaxLength(10)]
        public string ShipKey { get; set; }

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

        [Column("UPDATE_USER_ID")]
        [MaxLength(200)]
        public string? UpdateUserId { get; set; }

        public static ShipSatellite Of(string shipKey, string satelliteId, string satelliteType, string userId)
        {
            ArgumentException.ThrowIfNullOrEmpty(shipKey);
            ArgumentException.ThrowIfNullOrEmpty(satelliteId);
            ArgumentException.ThrowIfNullOrEmpty(satelliteType);
            ArgumentException.ThrowIfNullOrEmpty(userId);

            return new()
            {
                ShipKey = shipKey,
                SatelliteId = satelliteId,
                SatelliteType = SatelliteTypesConverter.ParseFromRequest(satelliteType),
                IsUseSatellite = true,
                CreateUserId = userId
            };
        }

        public bool IsSkTelink()
        {
            return SatelliteType == SatelliteTypes.SkTelink;
        }

        public void Update(string satelliteId, string satelliteType, string userId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(satelliteId);
            ArgumentException.ThrowIfNullOrWhiteSpace(satelliteType);
            ArgumentException.ThrowIfNullOrWhiteSpace(userId);

            this.SatelliteId = satelliteId;
            this.SatelliteType = SatelliteTypesConverter.ParseFromRequest(satelliteType);
            this.UpdateUserId = userId;
        }
    }
}
