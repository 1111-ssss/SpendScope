using Domain.Entities;
using Domain.ValueObjects;
using Domain.Abstractions.Interfaces;
using Ardalis.Specification;

namespace Application.Abstractions.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user, CancellationToken ct = default);
        Task<User?> GetByIdAsync(EntityId<User> id, CancellationToken ct = default);
        Task<User?> FindSingleAsync(ISpecification<User> spec, CancellationToken ct = default);
        Task<bool> AnyAsync(ISpecification<User> spec, CancellationToken ct = default);
    }
}