using Microsoft.EntityFrameworkCore;
using ShipParticularsApi.Entities;
using ShipParticularsApi.ValueConverters;

namespace ShipParticularsApi.Contexts
{
    public class ShipParticularsContext(DbContextOptions<ShipParticularsContext> options) : DbContext(options)
    {
        public DbSet<ReplaceShipName> ReplaceShipNames { get; set; }
        public DbSet<ShipInfo> ShipInfos { get; set; }
        public DbSet<ShipModelTest> ShipModelTests { get; set; }
        public DbSet<ShipSatellite> ShipSatellites { get; set; }
        public DbSet<ShipService> ShipServices { get; set; }
        public DbSet<SkTelinkCompanyShip> SkTelinkCompanyShips { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ReplaceShipName>(entity =>
            {
                entity.HasOne(child => child.ShipInfo)
                .WithOne(parent => parent.ReplaceShipName)
                .HasForeignKey<ReplaceShipName>(child => child.ShipKey)
                .HasPrincipalKey<ShipInfo>(parent => parent.ShipKey);
            });

            modelBuilder.Entity<ShipInfo>(entity =>
            {
                entity.Property(p => p.ShipType)
                    .HasDefaultValue(ShipTypes.Default)
                    .HasConversion<ShipTypesToStringConverter>();

                entity.Property(p => p.IsUseKtsat)
                    .HasDefaultValue(false);

                entity.Property(p => p.IsUseAis)
                    .HasDefaultValue(false);

                entity.Property(p => p.IsService)
                    .HasDefaultValue(true);
            });

            modelBuilder.Entity<ShipModelTest>(entity =>
            {
                entity.HasOne(child => child.ShipInfo)
                .WithOne(parent => parent.ShipModelTest)
                .HasForeignKey<ShipModelTest>(child => child.ShipKey)
                .HasPrincipalKey<ShipInfo>(parent => parent.ShipKey);
            });

            modelBuilder.Entity<ShipSatellite>(entity =>
            {
                entity.HasOne(child => child.ShipInfo)
                .WithOne(parent => parent.ShipSatellite)
                .HasForeignKey<ShipSatellite>(child => child.ShipKey)
                .HasPrincipalKey<ShipInfo>(parent => parent.ShipKey);

                //entity.Property(p => p.CreateDateTime)
                //    .HasDefaultValueSql("SYSDATETIME()");

                // TODO. NONE은 default value가 아니다. NONE은 입력값 자체가 안오도록 한다고 함
                entity.Property(p => p.SatelliteType)
                    .HasDefaultValue(SatelliteTypes.None)
                    .HasConversion<SatelliteTypesToStringConverter>();
            });

            modelBuilder.Entity<ShipService>(entity =>
            {
                entity.HasOne(child => child.ShipInfo)
                .WithMany(parent => parent.ShipServices)
                .HasForeignKey(child => child.ShipKey)
                .HasPrincipalKey(parent => parent.ShipKey);

                entity.Property(p => p.ServiceName)
                    .HasConversion<ServiceNameToStringConverter>();

                entity.Property(p => p.IsCompleted)
                    .HasDefaultValue(false);
            });

            modelBuilder.Entity<SkTelinkCompanyShip>(entity =>
            {
                entity.HasOne(child => child.ShipInfo)
                .WithOne(parent => parent.SkTelinkCompanyShip)
                .HasForeignKey<SkTelinkCompanyShip>(child => child.ShipKey)
                .HasPrincipalKey<ShipInfo>(parent => parent.ShipKey);
            });

        }
    }
}
