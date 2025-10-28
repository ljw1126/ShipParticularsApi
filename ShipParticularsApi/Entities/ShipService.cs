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
        public long Id { get; set; }

        [Column("SHIP_KEY")]
        [MaxLength(10)]
        public string ShipKey { get; set; }

        [Column("SERVICE_NAME")]
        [MaxLength(100)]
        public ServiceNameTypes? ServiceName { get; set; }

        [Column("IS_COMPLETED")]
        public bool IsCompleted { get; set; }

        public static ShipService Of(string shipKey, ServiceNameTypes serviceName)
        {
            ArgumentException.ThrowIfNullOrEmpty(shipKey);

            return new()
            {
                ShipKey = shipKey,
                ServiceName = serviceName,
                IsCompleted = true
            };
        }
    }
}
