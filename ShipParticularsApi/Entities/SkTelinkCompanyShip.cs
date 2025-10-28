using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShipParticularsApi.Entities
{
    [Table("SK_TELINK_COMPANY_SHIP")]
    public class SkTelinkCompanyShip
    {
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; private set; }

        [Required]
        [Column("SHIP_KEY")]
        [MaxLength(10)]
        public string ShipKey { get; private set; }

        [Required]
        [Column("COMPANY_NAME")]
        [MaxLength(100)]
        public string CompanyName { get; private set; }

        public SkTelinkCompanyShip(string shipKey, string companyName)
            : this(0L, shipKey, companyName)
        {
        }

        public SkTelinkCompanyShip(long id, string shipKey, string companyName)
        {
            Id = id;
            ShipKey = shipKey;
            CompanyName = companyName;
        }

        public static SkTelinkCompanyShip Of(string shipKey, string companyName)
        {
            ArgumentException.ThrowIfNullOrEmpty(shipKey);

            return new(shipKey, companyName);
        }

        public void Update(string companyName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(companyName);

            this.CompanyName = companyName;
        }
    }
}
