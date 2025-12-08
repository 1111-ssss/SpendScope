using Domain.Model.EntityModels;

namespace Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> ExistsByUsernameAsync(string username, CancellationToken ct = default);
        Task AddAsync(UserModel user, CancellationToken ct = default);
        Task<UserModel?> GetByIdAsync(int id, CancellationToken ct = default);
    }
}
