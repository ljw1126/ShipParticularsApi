using System.ComponentModel.DataAnnotations.Schema;

namespace ShipParticularsApi.Entities
{
    public class BaseEntity
    {
        [Column("CREATE_DATE_TIME")]
        public DateTime CreateDateTime { get; set; }

        [Column("UPDATE_DATE_TIME")]
        public DateTime? UpdateDateTime { get; set; }
    }
}
