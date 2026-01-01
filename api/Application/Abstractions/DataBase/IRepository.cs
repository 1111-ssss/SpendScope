// using System.Linq.Expressions;
// using Ardalis.Specification;
// using Domain.Abstractions.Interfaces;
// using Domain.ValueObjects;

// namespace Application.Abstractions.Interfaces
// {
//     public interface IRepository<T> where T : class, IAggregateRoot
//     {
//         Task AddAsync(T entity, CancellationToken ct = default);
//         Task<int> DeleteWhereAsync(ISpecification<T> spec, CancellationToken ct = default);
//         Task<T?> GetByIdAsync(EntityId<T> id, CancellationToken ct = default);
//         Task<T?> GetByIdAsync(EntityId<T> id, Expression<Func<T, object>>[] includes, CancellationToken ct = default);
//         Task<T?> FindSingleAsync(ISpecification<T> spec, CancellationToken ct = default);
//         Task UpdateAsync(T entity, CancellationToken ct = default);
//         Task<bool> AnyAsync(ISpecification<T> spec, CancellationToken ct = default);
//         Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec, CancellationToken ct = default);
//     }
// }