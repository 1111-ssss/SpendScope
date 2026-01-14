using Application.Abstractions.Auth;
using Application.Abstractions.DataBase;
using Application.Abstractions.Repository;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications.Auth;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Auth.Refresh;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<AuthResponse>>
{
    private readonly IUnitOfWork _uow;
    private readonly IBaseRepository<User> _userRepository;
    private readonly IBaseRepository<RefreshToken> _refreshTokenRepository;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly ILogger<RefreshTokenCommandHandler> _logger;
    private readonly ICurrentUserService _currentUserService;
    public RefreshTokenCommandHandler(
        IUnitOfWork uow,
        IJwtGenerator jwtGenerator,
        IBaseRepository<User> userRepository,
        IBaseRepository<RefreshToken> refreshTokenRepository,
        ILogger<RefreshTokenCommandHandler> logger,
        ICurrentUserService currentUserService)
    {
        _uow = uow;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtGenerator = jwtGenerator;
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<Result<AuthResponse>> Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        string userIp = _currentUserService.GetUserIp();
        int? userId = _currentUserService.GetUserId();
        if (userId == null)
            return Result.Unauthorized();

        var refreshToken = await _refreshTokenRepository.FirstOrDefaultAsync(new RefreshTokenByUserSpec(request.RefreshToken, userId.Value), ct);
        if (refreshToken == null)
            return Result.BadRequest("Токен обновления не найден");

        var tokenIsValid = refreshToken.Validate(request.RefreshToken);
        if (!tokenIsValid.IsSuccess)
            return Result.BadRequest(tokenIsValid.Message!);

        var user = await _userRepository.GetByIdAsync((EntityId<User>)userId.Value, ct);
        if (user == null)
            return Result.BadRequest("Пользователь не найден");

        // refreshToken.Revoke();
        await _refreshTokenRepository.DeleteAsync(refreshToken, ct);

        var result = _jwtGenerator.GenerateToken(user);
        if (!result.IsSuccess)
            return Result.InternalServerError("Ошибка при создании токена обновления");

        var newRefreshToken = RefreshToken.Create(
            result.Value.RefreshToken,
            DateTime.UtcNow.AddDays(7),
            userIp,
            user.Id
        );

        await _refreshTokenRepository.AddAsync(newRefreshToken, ct);

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