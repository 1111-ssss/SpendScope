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
        protected readonly AppDbContext Db;
        protected RepositoryBase(AppDbContext db)
        {
            Db = db;
        }
        protected DbSet<T> Set => Db.Set<T>();
        public virtual Task AddAsync(T entity, CancellationToken ct = default)
        {
            Set.Add(entity);
            return Task.CompletedTask;
        }
        public virtual Task<T?> GetByIdAsync(EntityId<T> id, CancellationToken ct = default)
        {
            return Set.FirstOrDefaultAsync(e => EF.Property<EntityId<T>>(e, "Id") == id, ct);
        }
        public virtual Task<bool> AnyAsync(ISpecification<T> spec, CancellationToken ct = default)
        {
            var query = SpecificationEvaluator.Default.GetQuery(Set.AsQueryable(), spec);
            return query.AnyAsync(ct);
        }
        public virtual Task<T?> FindSingleAsync(ISpecification<T> spec, CancellationToken ct = default)
        {
            var query = SpecificationEvaluator.Default.GetQuery(Set.AsQueryable(), spec);
            return query.FirstOrDefaultAsync(ct);
        }
        public virtual async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec, CancellationToken ct = default)
        {
            var query = SpecificationEvaluator.Default.GetQuery(Set.AsQueryable(), spec);
            return await query.ToListAsync(ct);
        }
    }
}