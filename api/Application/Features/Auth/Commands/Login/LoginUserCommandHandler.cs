using Application.Abstractions.DataBase;
using Application.Abstractions.Repository;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications.Auth;
using Application.Abstractions.Auth;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Auth.Login;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<AuthResponse>>
{
    private readonly IUnitOfWork _uow;
    private readonly IBaseRepository<User> _userRepository;
    private readonly IBaseRepository<RefreshToken> _refreshTokenRepository;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<LoginUserCommandHandler> _logger;
    private readonly ICurrentUserService _currentUserService;
    public LoginUserCommandHandler(
        IUnitOfWork uow,
        IJwtGenerator jwtGenerator,
        IBaseRepository<User> userRepository,
        IBaseRepository<RefreshToken> refreshTokenRepository,
        IPasswordHasher passwordHasher,
        ILogger<LoginUserCommandHandler> logger,
        ICurrentUserService currentUserService)
    {
        _uow = uow;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtGenerator = jwtGenerator;
        _passwordHasher = passwordHasher;
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<Result<AuthResponse>> Handle(LoginUserCommand request, CancellationToken ct)
    {
        var user = await _userRepository.FirstOrDefaultAsync(new UserByUsernameOrEmailSpec(request.Identifier), ct);

        if (user == null)
            return Result.BadRequest("Неверный логин или пароль");

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash).IsSuccess)
            return Result.BadRequest("Неверный логин или пароль");

        var result = _jwtGenerator.GenerateToken(user);
        if (!result.IsSuccess)
            return Result.InternalServerError("Ошибка генерации токена");

        var refreshTokenEntity = await _refreshTokenRepository.FirstOrDefaultAsync(new RefreshTokenByUserSpec(user.Id), ct);
        if (refreshTokenEntity != null)
        {
            await _refreshTokenRepository.DeleteAsync(refreshTokenEntity, ct);
        }
        
        var refreshToken = RefreshToken.Create(
            result.Value.RefreshToken,
            DateTime.UtcNow.AddDays(7),
            _currentUserService.GetUserIp(),
            user.Id
        );

        await _refreshTokenRepository.AddAsync(refreshToken, ct);

        try {
            await _uow.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при сохранении токена обновления");
            return Result.InternalServerError("Ошибка при сохранении токена обновления");
        }
        
        return result;
    }
}