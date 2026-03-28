using MES.Domain.Entities;
using MES.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace MES.Infrastructure.Persistence.Configurations
{
    public class WorkOrderConfiguration : IEntityTypeConfiguration<WorkOrder>
    {
        public void Configure(EntityTypeBuilder<WorkOrder> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.WorkOrderNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(x => x.WorkOrderNumber)
                .IsUnique();

            builder.Property(x => x.ProductCode)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.ProductName)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.PlannedQuantity)
                .HasColumnType("decimal(18,4)");

            builder.Property(x => x.ProducedQuantity).HasColumnType("decimal(18,4)");

            builder.Property(x => x.ScrappedQuantity).HasColumnType("decimal(18,4)");

            builder.Property(x => x.Status).HasConversion<string>();

            builder.Property(x => x.Notes).HasMaxLength(1000);

            builder.HasOne(x => x.WorkCentre)
                .WithMany(x => x.WorkOrders)
                .HasForeignKey(x => x.WorkCentreId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.ProductionEntries)
                .WithOne(x => x.WorkOrder)
                .HasForeignKey(x => x.WorkOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(x => !x.IsDeleted);

            builder.ToTable("WorkOrders");
        }
    }
}
