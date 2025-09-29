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

        public ShipInfo ShipInfo { get; set; }

        [Column("SERVICE_NAME")]
        [MaxLength(100)]
        public string ServiceName { get; set; }

        [Column("IS_COMPLETED")]
        public bool IsCompleted { get; set; }
    }
}
