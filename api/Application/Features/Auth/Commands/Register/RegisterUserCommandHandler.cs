using Application.Abstractions.DataBase;
using Application.Abstractions.Repository;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications.Auth;
using Application.Abstractions.Auth;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Auth.Register;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<AuthResponse>>
{
    private readonly IUnitOfWork _uow;
    private readonly IBaseRepository<User> _userRepository;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<RegisterUserCommandHandler> _logger;
    public RegisterUserCommandHandler(
        IUnitOfWork uow,
        IJwtGenerator jwtGenerator,
        IBaseRepository<User> userRepository,
        IPasswordHasher passwordHasher,
        ILogger<RegisterUserCommandHandler> logger)
    {
        _uow = uow;
        _userRepository = userRepository;
        _jwtGenerator = jwtGenerator;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task<Result<AuthResponse>> Handle(RegisterUserCommand request, CancellationToken ct)
    {
        var existingUser = await _userRepository.FirstOrDefaultAsync(
            new UserByUsernameOrEmailSpec(request.Username, request.Email), ct);

        if (existingUser != null)
        {
            var message = existingUser.Username == request.Username
                ? "Пользователь с таким логином уже существует"
                : "Пользователь с таким email уже существует";
            return Result<AuthResponse>.Failed(ErrorCode.BadRequest, message);
        }

        var passwordHash = _passwordHasher.Hash(request.Password);

        if (!passwordHash.IsSuccess)
            return Result<AuthResponse>.Failed(ErrorCode.BadRequest, "Ошибка хеширования пароля");

        var user = User.Create(request.Username, request.Email, passwordHash.Value);
        await _userRepository.AddAsync(user, ct);
        try
        {
            await _uow.SaveChangesAsync(ct);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка сохранения пользователя");
            return Result<AuthResponse>.Failed(ErrorCode.InternalServerError, "Ошибка сохранения пользователя");
        }

        var result = _jwtGenerator.GenerateToken(user);

        if (!result.IsSuccess)
            return Result<AuthResponse>.Failed(ErrorCode.InternalServerError, "Ошибка генерации токена");

        return result;
    }
}