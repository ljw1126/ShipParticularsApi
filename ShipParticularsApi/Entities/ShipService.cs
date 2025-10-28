using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShipParticularsApi.Entities
{
    [Table("SHIP_SERVICE")]
    public class ShipService
    {
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; private set; }

        [Column("SHIP_KEY")]
        [MaxLength(10)]
        public string ShipKey { get; private set; }

        [Column("SERVICE_NAME")]
        [MaxLength(100)]
        public ServiceNameTypes? ServiceName { get; private set; }

        [Column("IS_COMPLETED")]
        public bool IsCompleted { get; private set; }

        public ShipService(string shipKey, ServiceNameTypes? serviceName, bool isCompleted)
            : this(0L, shipKey, serviceName, isCompleted)
        {
        }

        public ShipService(long id, string shipKey, ServiceNameTypes? serviceName, bool isCompleted)
        {
            Id = id;
            ShipKey = shipKey;
            ServiceName = serviceName;
            IsCompleted = isCompleted;
        }

        public static ShipService Of(string shipKey, ServiceNameTypes serviceName)
        {
            ArgumentException.ThrowIfNullOrEmpty(shipKey);

            return new(shipKey, serviceName, true);
        }
    }
}
