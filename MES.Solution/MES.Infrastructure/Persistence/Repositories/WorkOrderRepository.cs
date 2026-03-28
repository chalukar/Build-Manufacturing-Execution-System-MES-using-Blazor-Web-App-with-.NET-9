using MES.Domain.Entities;
using MES.Domain.Enums;
using MES.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace MES.Infrastructure.Persistence.Repositories
{
    public class WorkOrderRepository(MesDbContext context): RepositoryBase<WorkOrder>(context), IWorkOrderRepository
    {
        public async Task<WorkOrder?> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
        => await Context.WorkOrders
            .Include(x => x.WorkCentre)
            .Include(x => x.ProductionEntries)
            .Include(x => x.QualityInspections)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        public async Task<IReadOnlyList<WorkOrder>> GetByStatusAsync(WorkOrderStatus status, CancellationToken cancellationToken = default)
            => await Context.WorkOrders
                .Include(x => x.WorkCentre)
                .Where(x => x.Status == status)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);

        public async Task<bool> ExistsAsync(string workOrderNumber, CancellationToken cancellationToken = default)
            => await Context.WorkOrders.AnyAsync(x => x.WorkOrderNumber == workOrderNumber, cancellationToken);
    }
}

