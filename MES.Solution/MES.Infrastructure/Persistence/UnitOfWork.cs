using MES.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.Infrastructure.Persistence
{
    public class UnitOfWork(MesDbContext context,IWorkOrderRepository workOrders,
    IWorkCentreRepository workCentres) : IUnitOfWork
    {
        public IWorkOrderRepository WorkOrders { get; } = workOrders;
        public IWorkCentreRepository WorkCentres { get; } = workCentres;

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => await context.SaveChangesAsync(cancellationToken);

        public void Dispose() => context.Dispose();
    }
}
