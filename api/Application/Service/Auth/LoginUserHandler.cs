using Application.DTO.Auth;
using Infrastructure.Interfaces;
using Domain.Model.Result;
using Microsoft.EntityFrameworkCore;
using Application.Abstractions.Auth;
using Domain.Model.EntityModels;
using Domain.Model.Enums;

namespace Application.Service.Auth
{
    public class LoginUserHandler
    {
        private readonly IAppDbContext _db;
        private readonly IPasswordHasher _hasher;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IUserRepository _users;

        public LoginUserHandler(IAppDbContext db, IPasswordHasher hasher, IJwtGenerator jwtGenerator, IUserRepository users)
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

            UserModel? user = null;

            switch (request.LoginMethod)
            {
                case LoginMethod.Email:
                    user = await _users.GetByEmailAsync(request.UsernameOrEmail);
                    break;
                case LoginMethod.Username:
                    user = await _users.GetByUsernameAsync(request.UsernameOrEmail);
                    break;
            }

            if (user == null)
                return Result<string>.Failed(ErrorCode.Conflict, "Пользователь не найден");


            bool verifyPass = _hasher.Verify(request.Password, user.PasswordHash);

            if (!verifyPass)
                return Result<string>.Failed(ErrorCode.Unauthorized, "Неверное имя пользователя или пароль");

            var jwt = _jwtGenerator.GenerateToken(user.Id, user.Username, user.IsAdmin ?? false);
            return Result<string>.Success(jwt);
        }
    }
}