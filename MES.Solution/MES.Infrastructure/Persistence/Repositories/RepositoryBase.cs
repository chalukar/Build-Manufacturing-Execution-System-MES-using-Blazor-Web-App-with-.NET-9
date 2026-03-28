using MES.Domain.Common;
using MES.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MES.Infrastructure.Persistence.Repositories
{
    public abstract class RepositoryBase<T>(MesDbContext context) : IRepository<T> where T : BaseEntity
    {
        protected readonly MesDbContext Context = context;

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await Context.Set<T>().FindAsync([id], cancellationToken);

        public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
            => await Context.Set<T>().ToListAsync(cancellationToken);

        public async Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => await Context.Set<T>().Where(predicate).ToListAsync(cancellationToken);

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
            => await Context.Set<T>().AddAsync(entity, cancellationToken);

        public void Update(T entity) => Context.Set<T>().Update(entity);

        public void Remove(T entity) => Context.Set<T>().Remove(entity);
    }
}
