using Application.DTO.Auth;
using Infrastructure.Interfaces;
using Domain.Model.Result;
using Microsoft.EntityFrameworkCore;
using Application.Abstractions.Auth;
using Domain.Model.EntityModels;

namespace Application.Service.Auth
{
    public class RegisterUserHandler
    {
        private readonly IAppDbContext _db;
        private readonly IPasswordHasher _hasher;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IUserRepository _users;

        public RegisterUserHandler(IAppDbContext db, IPasswordHasher hasher, IJwtGenerator jwtGenerator, IUserRepository users)
        {
            _db = db;
            _hasher = hasher;
            _jwtGenerator = jwtGenerator;
            _users = users;
        }
        public async Task<Result<string>> Handle(RegisterUserRequest request, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(request.Username))
                return Result<string>.Failed(ErrorCode.ValidationFailed, "Username обязателен");

            if (await _users.ExistsByUsernameAsync(request.Username))
                return Result<string>.Failed(ErrorCode.Conflict, "Пользователь уже существует");

            if (!string.IsNullOrWhiteSpace(request.Email) && await _users.ExistsByEmailAsync(request.Email))
                return Result<string>.Failed(ErrorCode.Conflict, "Email уже используется");

            Result<string> passwordHash = _hasher.Hash(request.Password);

            if (!passwordHash.IsSuccess)
                return Result<string>.Failed(ErrorCode.ValidationFailed, "Ошибка хеширования пароля");

            var user = new UserModel
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = passwordHash.Value,
                CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified)
            };

            await _users.AddAsync(user);
            await _db.SaveChangesAsync();

            var jwt = _jwtGenerator.GenerateToken(user.Id, user.Username, user.IsAdmin ?? false);
            return Result<string>.Success(jwt);
        }
    }
}