using MES.Domain.Entities;
using MES.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.Infrastructure.Persistence.Repositories
{
    public class WorkCentreRepository(MesDbContext context)
    : RepositoryBase<WorkCentre>(context), IWorkCentreRepository
    {
        public async Task<WorkCentre?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
            => await Context.WorkCentres
                .FirstOrDefaultAsync(x => x.Code == code, cancellationToken);
    }
}
