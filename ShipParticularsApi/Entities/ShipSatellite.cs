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
        public long Id { get; private set; }

        [Required]
        [Column("SHIP_KEY")]
        [MaxLength(10)]
        public string ShipKey { get; private set; }

        [Column("SATELLITE_TYPE")]
        [MaxLength(200)]
        public SatelliteTypes? SatelliteType { get; private set; }

        [Column("SATELLITE_ID")]
        [MaxLength(256)]
        public string? SatelliteId { get; private set; }

        [Column("IS_USE_SATELLITE")]
        public bool IsUseSatellite { get; private set; }

        [Column("CREATE_USER_ID")]
        [MaxLength(200)]
        public string? CreateUserId { get; private set; }

        [Column("UPDATE_USER_ID")]
        [MaxLength(200)]
        public string? UpdateUserId { get; private set; }

        public ShipSatellite(string shipKey, SatelliteTypes? satelliteType, string? satelliteId, bool isUseSatellite, string? createUserId)
            : this(0L, shipKey, satelliteType, satelliteId, isUseSatellite, createUserId, null)
        {
        }

        public ShipSatellite(long id, string shipKey, SatelliteTypes? satelliteType, string? satelliteId, bool isUseSatellite, string? createUserId, string? updateUserId)
        {
            Id = id;
            ShipKey = shipKey;
            SatelliteType = satelliteType;
            SatelliteId = satelliteId;
            IsUseSatellite = isUseSatellite;
            CreateUserId = createUserId;
            UpdateUserId = updateUserId;
        }

        public static ShipSatellite Of(string shipKey, string satelliteId, string satelliteType, string userId)
        {
            ArgumentException.ThrowIfNullOrEmpty(shipKey);
            ArgumentException.ThrowIfNullOrEmpty(satelliteId);
            ArgumentException.ThrowIfNullOrEmpty(satelliteType);
            ArgumentException.ThrowIfNullOrEmpty(userId);

            return new(shipKey, SatelliteTypesConverter.ParseFromRequest(satelliteType), satelliteId, true, userId);
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
