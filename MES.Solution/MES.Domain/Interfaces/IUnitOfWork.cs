using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IWorkOrderRepository WorkOrders { get; }
        IWorkCentreRepository WorkCentres { get; }
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
