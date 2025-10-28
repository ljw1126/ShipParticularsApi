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
        public long Id { get; private set; }

        [Column("SHIP_KEY")]
        [Required]
        [MaxLength(10)]
        public string ShipKey { get; private set; }

        [Column("ZA_BALLAST")]
        public double ZaBallast { get; private set; }

        [Column("TRANSVERSE_PROJECTION_AREA_BALLAST")]
        public double TransverseProjectionAreaBallast { get; private set; }

        [Column("TRANSVERSE_PROJECTION_AREA_SCANTLIN")]
        public double TransverseProjectionAreaScantling { get; private set; }

        [Column("KYY")]
        public double Kyy { get; private set; }

        [Column("DRAFT_FORE")]
        public double DraftFore { get; private set; }

        [Column("DRAFT_AFT")]
        public double DraftAft { get; private set; }

        [Column("CB_BALLAST")]
        public double CbBallast { get; private set; }

        [Column("CB_SCANTLING")]
        public double CbScantling { get; private set; }

        [Column("SUBMERGED_SURFACE_BALLAST")]
        public double SubmergedSurfaceBallast { get; private set; }

        [Column("SUBMERGED_SURFACE_SCANTLING")]
        public double SubmergedSurfaceScantling { get; private set; }

        [Column("MID_SHIP_SECTION_AREA_BALLAST")]
        public double MidShipSectionAreaBallast { get; private set; }

        [Column("MID_SHIP_SECTION_AREA_SCANTLING")]
        public double MidShipSectionAreaScantling { get; private set; }

        [Column("DISPLACEMENT_BALLAST")]
        public double DisplacementBallast { get; private set; }

        [Column("DISPLACEMENT_SCANTLING")]
        public double DisplacementScantling { get; private set; }

        [Column("SPEED_etaD_BALLAST")]
        public string SpeedEtaDBallast { get; private set; }

        [Column("etaD_BALLAST")]
        public string EtaDBallast { get; private set; }

        [Column("SPEED_etaD_SCANTLING")]
        public string SpeedEtaDScantling { get; private set; }

        [Column("etaD_SCANTLING")]
        public string EtaDScantling { get; private set; }

        public ShipModelTest(
            string shipKey,
            double zaBallast,
            double transverseProjectionAreaBallast,
            double transverseProjectionAreaScantling,
            double kyy,
            double draftFore,
            double draftAft,
            double cbBallast,
            double cbScantling,
            double submergedSurfaceBallast,
            double submergedSurfaceScantling,
            double midShipSectionAreaBallast,
            double midShipSectionAreaScantling,
            double displacementBallast,
            double displacementScantling,
            string speedEtaDBallast,
            string etaDBallast,
            string speedEtaDScantling,
            string etaDScantling
        ) : this(
            0L,
            shipKey,
            zaBallast,
            transverseProjectionAreaBallast,
            transverseProjectionAreaScantling,
            kyy,
            draftFore,
            draftAft,
            cbBallast,
            cbScantling,
            submergedSurfaceBallast,
            submergedSurfaceScantling,
            midShipSectionAreaBallast,
            midShipSectionAreaScantling,
            displacementBallast,
            displacementScantling,
            speedEtaDBallast,
            etaDBallast,
            speedEtaDScantling,
            etaDScantling
        )
        {
        }

        public ShipModelTest(
            long id,
            string shipKey,
            double zaBallast,
            double transverseProjectionAreaBallast,
            double transverseProjectionAreaScantling,
            double kyy,
            double draftFore,
            double draftAft,
            double cbBallast,
            double cbScantling,
            double submergedSurfaceBallast,
            double submergedSurfaceScantling,
            double midShipSectionAreaBallast,
            double midShipSectionAreaScantling,
            double displacementBallast,
            double displacementScantling,
            string speedEtaDBallast,
            string etaDBallast,
            string speedEtaDScantling,
            string etaDScantling
        )
        {
            Id = id;
            ShipKey = shipKey;
            ZaBallast = zaBallast;
            TransverseProjectionAreaBallast = transverseProjectionAreaBallast;
            TransverseProjectionAreaScantling = transverseProjectionAreaScantling;
            Kyy = kyy;
            DraftFore = draftFore;
            DraftAft = draftAft;
            CbBallast = cbBallast;
            CbScantling = cbScantling;
            SubmergedSurfaceBallast = submergedSurfaceBallast;
            SubmergedSurfaceScantling = submergedSurfaceScantling;
            MidShipSectionAreaBallast = midShipSectionAreaBallast;
            MidShipSectionAreaScantling = midShipSectionAreaScantling;
            DisplacementBallast = displacementBallast;
            DisplacementScantling = displacementScantling;
            SpeedEtaDBallast = speedEtaDBallast;
            EtaDBallast = etaDBallast;
            SpeedEtaDScantling = speedEtaDScantling;
            EtaDScantling = etaDScantling;
        }

        public static ShipModelTest From(string shipKey, ShipModelTestDetails data)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(shipKey);

            return new(
                shipKey,
                data.ZaBallast,
                data.TransverseProjectionAreaBallast,
                data.TransverseProjectionAreaScantling,
                data.Kyy,
                data.DraftFore,
                data.DraftAft,
                data.CbBallast,
                data.CbScantling,
                data.SubmergedSurfaceBallast,
                data.SubmergedSurfaceScantling,
                data.MidShipSectionAreaBallast,
                data.MidShipSectionAreaScantling,
                data.DisplacementBallast,
                data.DisplacementScantling,
                data.SpeedEtaDBallast,
                data.EtaDBallast,
                data.SpeedEtaDScantling,
                data.EtaDScantling
            );
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
