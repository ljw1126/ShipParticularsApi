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
        public long Id { get; set; }

        [Required]
        [Column("SHIP_KEY")]
        [MaxLength(10)]
        public string ShipKey { get; set; }

        [Required]
        [Column("COMPANY_NAME")]
        [MaxLength(100)]
        public string CompanyName { get; set; }

        public static SkTelinkCompanyShip Of(string shipKey, string companyName)
        {
            return new SkTelinkCompanyShip
            {
                ShipKey = shipKey,
                CompanyName = companyName
            };
        }

        public void Update(string companyName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(companyName);

            this.CompanyName = companyName;
        }
    }
}
