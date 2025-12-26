using Application.Abstractions.Interfaces;
using Domain.Abstractions.Interfaces;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class, IAggregateRoot
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;
        protected RepositoryBase(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public virtual Task AddAsync(T entity, CancellationToken ct = default)
        {
            _dbSet.Add(entity);
            return Task.CompletedTask;
        }
        public virtual Task<T?> GetByIdAsync(EntityId<T> id, CancellationToken ct = default)
        {
            return _dbSet.FirstOrDefaultAsync(e => EF.Property<EntityId<T>>(e, "Id") == id, ct);
        }
        public virtual Task UpdateAsync(T entity, CancellationToken ct = default)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }
        public virtual Task<bool> AnyAsync(ISpecification<T> spec, CancellationToken ct = default)
        {
            var query = SpecificationEvaluator.Default.GetQuery(_dbSet.AsQueryable(), spec);
            return query.AnyAsync(ct);
        }
        public virtual Task<T?> FindSingleAsync(ISpecification<T> spec, CancellationToken ct = default)
        {
            var query = SpecificationEvaluator.Default.GetQuery(_dbSet.AsQueryable(), spec);
            return query.FirstOrDefaultAsync(ct);
        }
        public virtual async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec, CancellationToken ct = default)
        {
            var query = SpecificationEvaluator.Default.GetQuery(_dbSet.AsQueryable(), spec);
            return await query.ToListAsync(ct);
        }
    }
}