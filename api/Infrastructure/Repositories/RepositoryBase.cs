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
            Console.WriteLine($"[DEBUG] Adding entity of type {typeof(T).Name}");
            var entry = _dbSet.Entry(entity);
            Console.WriteLine($"[DEBUG] State before Add: {entry.State}");

            _dbSet.Add(entity);

            entry = _dbSet.Entry(entity);
            Console.WriteLine($"[DEBUG] State after Add: {entry.State}");
            Console.WriteLine($"[DEBUG] Total tracked: {_context.ChangeTracker.Entries().Count()}");

            return Task.CompletedTask;
        }
        public virtual Task<T?> GetByIdAsync(EntityId<T> id, CancellationToken ct = default)
        {
            return _dbSet.FirstOrDefaultAsync(e => EF.Property<EntityId<T>>(e, "Id") == id, ct);
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