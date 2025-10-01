using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ShipParticularsApi.Entities
{
    [Table("REPLACE_SHIP_NAME")]
    public class ReplaceShipName
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

        [Column("REPLACED_SHIP_NAME")]
        public string ReplacedShipName { get; set; }
    }
}
