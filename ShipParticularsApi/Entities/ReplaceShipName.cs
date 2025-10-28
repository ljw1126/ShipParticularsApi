using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShipParticularsApi.ValueObjects;
namespace ShipParticularsApi.Entities
{
    [Table("REPLACE_SHIP_NAME")]
    public class ReplaceShipName
    {
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; private set; }

        [Required]
        [Column("SHIP_KEY")]
        [MaxLength(10)]
        public string ShipKey { get; private set; }

        [Column("REPLACED_SHIP_NAME")]
        public string ReplacedShipName { get; private set; }

        public ReplaceShipName(string shipKey, string replacedShipName)
            : this(0L, shipKey, replacedShipName)
        {
        }

        public ReplaceShipName(long id, string shipKey, string replacedShipName)
        {
            Id = id;
            ShipKey = shipKey;
            ReplacedShipName = replacedShipName;
        }

        public static ReplaceShipName From(string shipKey, ReplaceShipNameDetails data)
        {
            ArgumentException.ThrowIfNullOrEmpty(shipKey);

            return new(shipKey, data.ReplacedShipName);
        }

        public void Update(ReplaceShipNameDetails data)
        {
            this.ReplacedShipName = data.ReplacedShipName;
        }
    }
}
