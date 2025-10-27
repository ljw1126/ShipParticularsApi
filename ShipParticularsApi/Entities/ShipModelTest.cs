using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShipParticularsApi.ValueObjects;

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

        public static ShipModelTest From(string shipKey, ShipModelTestDetails data)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(shipKey);

            return new()
            {
                ShipKey = shipKey,
                ZaBallast = data.ZaBallast,
                TransverseProjectionAreaBallast = data.TransverseProjectionAreaBallast,
                TransverseProjectionAreaScantling = data.TransverseProjectionAreaScantling,
                Kyy = data.Kyy,
                DraftFore = data.DraftFore,
                DraftAft = data.DraftAft,
                CbBallast = data.CbBallast,
                CbScantling = data.CbScantling,
                SubmergedSurfaceBallast = data.SubmergedSurfaceBallast,
                SubmergedSurfaceScantling = data.SubmergedSurfaceScantling,
                MidShipSectionAreaBallast = data.MidShipSectionAreaBallast,
                MidShipSectionAreaScantling = data.MidShipSectionAreaScantling,
                DisplacementBallast = data.DisplacementBallast,
                DisplacementScantling = data.DisplacementScantling,
                SpeedEtaDBallast = data.SpeedEtaDBallast,
                EtaDBallast = data.EtaDBallast,
                SpeedEtaDScantling = data.SpeedEtaDScantling,
                EtaDScantling = data.EtaDScantling
            };
        }

        public void Update(ShipModelTestDetails data)
        {
            this.ZaBallast = data.ZaBallast;
            this.TransverseProjectionAreaBallast = data.TransverseProjectionAreaBallast;
            this.TransverseProjectionAreaScantling = data.TransverseProjectionAreaScantling;
            this.Kyy = data.Kyy;
            this.DraftFore = data.DraftFore;
            this.DraftAft = data.DraftAft;
            this.CbBallast = data.CbBallast;
            this.CbScantling = data.CbScantling;
            this.SubmergedSurfaceBallast = data.SubmergedSurfaceBallast;
            this.SubmergedSurfaceScantling = data.SubmergedSurfaceScantling;
            this.MidShipSectionAreaBallast = data.MidShipSectionAreaBallast;
            this.MidShipSectionAreaScantling = data.MidShipSectionAreaScantling;
            this.DisplacementBallast = data.DisplacementBallast;
            this.DisplacementScantling = data.DisplacementScantling;
            this.SpeedEtaDBallast = data.SpeedEtaDBallast;
            this.EtaDBallast = data.EtaDBallast;
            this.SpeedEtaDScantling = data.SpeedEtaDScantling;
            this.EtaDScantling = data.EtaDScantling;
        }
    }
}
