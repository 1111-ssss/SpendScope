using Application.DTO.Auth;
using Application.Abstractions.Repositories;
using Domain.Abstractions.Result;
using Application.Abstractions.Auth;
using Domain.Entities;
using Domain.Specifications;
using Application.Abstractions.Interfaces;

namespace Application.Service.Auth.Handlers
{
    public class RegisterUserHandler
    {
        private readonly IPasswordHasher _hasher;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IRepository<User> _users;
        private readonly IUnitOfWork _db;

        public RegisterUserHandler(IUnitOfWork db, IPasswordHasher hasher, IJwtGenerator jwtGenerator, IRepository<User> users)
        {
            _db = db;
            _hasher = hasher;
            _jwtGenerator = jwtGenerator;
            _users = users;
        }
        public async Task<Result<string>> Handle(RegisterUserRequest request, CancellationToken ct = default)
        {
            if (await _users.AnyAsync(new UserByUsernameSpecification(request.Username), ct))
                return Result<string>.Failed(ErrorCode.Conflict, "Username уже занят");

            if (!string.IsNullOrWhiteSpace(request.Email) &&
                await _users.AnyAsync(new UserByEmailSpecification(request.Email), ct))
                return Result<string>.Failed(ErrorCode.Conflict, "Email уже используется");

            var passwordHashResult = _hasher.Hash(request.Password);
            if (!passwordHashResult.IsSuccess)
                return Result<string>.Failed(ErrorCode.ValidationFailed, "Ошибка хеширования");

            var user = User.Create(
                username: request.Username,
                email: request.Email,
                passwordHash: passwordHashResult.Value,
                isAdmin: false
            );

            await _users.AddAsync(user, ct);
            await _db.SaveChangesAsync(ct);

            var token = _jwtGenerator.GenerateToken(user.Id, user.Username, user.IsAdmin);
            return Result<string>.Success(token);
        }
    }
}