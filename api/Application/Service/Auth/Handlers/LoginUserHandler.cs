using Application.DTO.Auth;
using Application.Abstractions.Interfaces;
using Domain.Abstractions.Result;
using Application.Abstractions.Auth;
using Domain.Entities;
using Domain.Enums;
using Domain.Specifications;

namespace Application.Service.Auth.Handlers
{
    public class LoginUserHandler
    {
        private readonly IUnitOfWork _db;
        private readonly IPasswordHasher _hasher;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IRepository<User> _users;
        public LoginUserHandler(IUnitOfWork db, IPasswordHasher hasher, IJwtGenerator jwtGenerator, IRepository<User> users)
        {
            _db = db;
            _hasher = hasher;
            _jwtGenerator = jwtGenerator;
            _users = users;
        }
        public async Task<Result<string>> Handle(LoginUserRequest request, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(request.UsernameOrEmail))
                return Result<string>.Failed(ErrorCode.ValidationFailed, "Username или Email обязателен");

            User? user = null;

            switch (request.LoginMethod)
            {
                case LoginMethod.Email:
                    user = await _users.FindSingleAsync(new UserByEmailSpecification(request.UsernameOrEmail), ct);
                    break;
                case LoginMethod.Username:
                    user = await _users.FindSingleAsync(new UserByUsernameSpecification(request.UsernameOrEmail), ct);
                    break;
            }

            if (user == null)
                return Result<string>.Failed(ErrorCode.Conflict, "Пользователь не найден");


            bool verifyPass = _hasher.Verify(request.Password, user.PasswordHash);

            if (!verifyPass)
                return Result<string>.Failed(ErrorCode.Unauthorized, "Неверное имя пользователя или пароль");

            var jwt = _jwtGenerator.GenerateToken(user.Id, user.Username, user.IsAdmin);
            return Result<string>.Success(jwt);
        }
    }
}