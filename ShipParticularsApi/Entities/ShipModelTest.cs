using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShipParticularsApi.Entities
{
    [Table("SHIP_MODEL_TEST")]
    public class ShipModelTest
    {
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("SHIP_KEY")]
        [Required]
        [MaxLength(10)]
        public string ShipKey { get; set; }

        public ShipInfo ShipInfo { get; set; }

        [Column("ZA_BALLAST")]
        public double ZaBallast { get; set; }

        [Column("TRANSVERSE_PROJECTION_AREA_BALLAST")]
        public double TransverseProjectionAreaBallast { get; set; }

        [Column("TRANSVERSE_PROJECTION_AREA_SCANTLIN")]
        public double TransverseProjectionAreaScantling { get; set; }

        [Column("KYY")]
        public double Kyy { get; set; }

        [Column("DRAFT_FORE")]
        public double DraftFore { get; set; }

        [Column("DRAFT_AFT")]
        public double DraftAft { get; set; }

        [Column("CB_BALLAST")]
        public double CbBallast { get; set; }

        [Column("CB_SCANTLING")]
        public double CbScantling { get; set; }

        [Column("SUBMERGED_SURFACE_BALLAST")]
        public double SubmergedSurfaceBallast { get; set; }

        [Column("SUBMERGED_SURFACE_SCANTLING")]
        public double SubmergedSurfaceScantling { get; set; }

        [Column("MID_SHIP_SECTION_AREA_BALLAST")]
        public double MidShipSectionAreaBallast { get; set; }

        [Column("MID_SHIP_SECTION_AREA_SCANTLING")]
        public double MidShipSectionAreaScantling { get; set; }

        [Column("DISPLACEMENT_BALLAST")]
        public double DisplacementBallast { get; set; }

        [Column("DISPLACEMENT_SCANTLING")]
        public double DisplacementScantling { get; set; }

        [Column("SPEED_etaD_BALLAST")]
        public string SpeedEtaDBallast { get; set; }

        [Column("etaD_BALLAST")]
        public string EtaDBallast { get; set; }

        [Column("SPEED_etaD_SCANTLING")]
        public string SpeedEtaDScantling { get; set; }

        [Column("etaD_SCANTLING")]
        public string EtaDScantling { get; set; }
    }
}
