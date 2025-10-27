using Microsoft.EntityFrameworkCore;
using ShipParticularsApi.Entities;
using ShipParticularsApi.Entities.Enums;
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
                entity.HasOne<ShipInfo>()
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
                entity.HasOne<ShipInfo>()
                .WithOne(parent => parent.ShipModelTest)
                .HasForeignKey<ShipModelTest>(child => child.ShipKey)
                .HasPrincipalKey<ShipInfo>(parent => parent.ShipKey);
            });

            modelBuilder.Entity<ShipSatellite>(entity =>
            {
                entity.HasOne<ShipInfo>()
                .WithOne(parent => parent.ShipSatellite)
                .HasForeignKey<ShipSatellite>(child => child.ShipKey)
                .HasPrincipalKey<ShipInfo>(parent => parent.ShipKey);

                entity.Property(p => p.SatelliteType)
                    .HasConversion<SatelliteTypesToStringConverter>();
            });

            modelBuilder.Entity<ShipService>(entity =>
            {
                entity.HasOne<ShipInfo>()
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
                entity.HasOne<ShipInfo>()
                .WithOne(parent => parent.SkTelinkCompanyShip)
                .HasForeignKey<SkTelinkCompanyShip>(child => child.ShipKey)
                .HasPrincipalKey<ShipInfo>(parent => parent.ShipKey);
            });
        }

        // 참고. https://www.entityframeworktutorial.net/faq/set-created-and-modified-date-in-efcore.aspx
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity
                && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entryEntity in entries)
            {
                if (entryEntity.State == EntityState.Added)
                {
                    ((BaseEntity)entryEntity.Entity).CreateDateTime = DateTime.UtcNow;
                }
                else
                {
                    ((BaseEntity)entryEntity.Entity).UpdateDateTime = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
