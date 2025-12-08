using Infrastructure.Interfaces;
using Infrastructure.Entities;
using Domain.Model.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IAppDbContext _db;
        public UserRepository(IAppDbContext db) => _db = db;
        public Task<bool> ExistsByUsernameAsync(string username, CancellationToken ct = default) =>
            _db.Users.AnyAsync(u => u.Username == username, ct);
        public Task AddAsync(UserModel user, CancellationToken ct = default)
        {
            var entity = new User
            {
                Username = user.Username,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                CreatedAt = user.CreatedAt,
                IsAdmin = user.IsAdmin
            };

            _db.Users.Add(entity);
            return Task.CompletedTask;
        }
        public Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default) =>
            _db.Users.AnyAsync(u => u.Email == email, ct);
        public async Task<UserModel?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var e = await _db.Users.FindAsync(new object[] { id }, ct);
            if (e == null) return null;
            return new UserModel
            {
                Id = e.Id,
                Username = e.Username,
                Email = e.Email,
                PasswordHash = e.PasswordHash,
                CreatedAt = e.CreatedAt,
                IsAdmin = e.IsAdmin
            };
        }

        public async Task<UserModel?> GetByEmailAsync(string email, CancellationToken ct = default)
        {
            var e = await _db.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
            if (e == null) return null;
            return new UserModel
            {
                Id = e.Id,
                Username = e.Username,
                Email = e.Email,
                PasswordHash = e.PasswordHash,
                CreatedAt = e.CreatedAt,
                IsAdmin = e.IsAdmin
            };
        }

        public async Task<UserModel?> GetByUsernameAsync(string username, CancellationToken ct = default)
        {
            var e = await _db.Users.FirstOrDefaultAsync(u => u.Username == username, ct);
            if (e == null) return null;
            return new UserModel
            {
                Id = e.Id,
                Username = e.Username,
                Email = e.Email,
                PasswordHash = e.PasswordHash,
                CreatedAt = e.CreatedAt,
                IsAdmin = e.IsAdmin
            };
        }
    }
}
