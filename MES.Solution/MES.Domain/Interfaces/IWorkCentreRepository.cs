using MES.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.Domain.Interfaces
{
    public interface IWorkCentreRepository : IRepository<WorkCentre>
    {
        Task<WorkCentre?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    }
}
