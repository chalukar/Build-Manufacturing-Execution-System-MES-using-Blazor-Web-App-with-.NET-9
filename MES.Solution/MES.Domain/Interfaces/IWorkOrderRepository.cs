using MES.Domain.Entities;
using MES.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.Domain.Interfaces
{
    public interface IWorkOrderRepository : IRepository<WorkOrder>
    {
        Task<WorkOrder?> GetWithDetailsAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyList<WorkOrder>> GetByStatusAsync(WorkOrderStatus status, CancellationToken ct = default);
        Task<bool> ExistsAsync(string workOrderNumber, CancellationToken ct = default);
    }
}
