using MES.Domain.Common;
using MES.Domain.Entities;
using Microsoft.EntityFrameworkCore;



namespace MES.Infrastructure.Persistence
{
    public class MesDbContext(DbContextOptions<MesDbContext> options) : DbContext(options)
    {
        public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();
        public DbSet<WorkCentre> WorkCentres => Set<WorkCentre>();
        public DbSet<ProductionEntry> ProductionEntries => Set<ProductionEntry>();
        public DbSet<QualityInspection> QualityInspections => Set<QualityInspection>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MesDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Modified)
                    entry.Entity.SetUpdatedBy("system"); // replace with actual user context
            }
            return await base.SaveChangesAsync(ct);
        }
    }
}
