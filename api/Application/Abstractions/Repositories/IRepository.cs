using Domain.Abstractions.Interfaces;
using Domain.ValueObjects;

namespace Application.Abstractions.Interfaces
{
    public interface IRepository<T> where T : class, IAggregateRoot
    {
        Task AddAsync(T entity, CancellationToken ct = default);
        Task<T?> GetByIdAsync(EntityId<T> id, CancellationToken ct = default);
        Task<T?> FindSingleAsync(Ardalis.Specification.ISpecification<T> spec, CancellationToken ct = default);
        Task<bool> AnyAsync(Ardalis.Specification.ISpecification<T> spec, CancellationToken ct = default);
        Task<IReadOnlyList<T>> ListAsync(Ardalis.Specification.ISpecification<T> spec, CancellationToken ct = default);
    }
}