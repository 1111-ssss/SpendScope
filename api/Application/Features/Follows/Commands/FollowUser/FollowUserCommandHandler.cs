using Application.Abstractions.Auth;
using Application.Abstractions.DataBase;
using Application.Abstractions.Repository;
using Domain.Abstractions.Result;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Follows.FollowUser
{
    public class FollowUserCommandHandler : IRequestHandler<FollowUserCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        private readonly IBaseRepository<Follow> _followRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<FollowUserCommandHandler> _logger;
        public FollowUserCommandHandler(
            IUnitOfWork uow,
            IBaseRepository<Follow> followRepository,
            ICurrentUserService currentUserService,
            ILogger<FollowUserCommandHandler> logger)
        {
            _uow = uow;
            _followRepository = followRepository;
            _currentUserService = currentUserService;
            _logger = logger;
        }
        public async Task<Result> Handle(FollowUserCommand request, CancellationToken ct)
        {
            var userId = _currentUserService.GetUserId();
            if (userId == null)
                return Result.Failed(ErrorCode.Unauthorized, "Не удалось определить пользователя");

            await _followRepository.AddAsync(
                Follow.Create(userId.Value, request.UserId),
                ct
            );
            try
            {
                await _uow.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при добавлении подписки на пользователя");
                return Result.Failed(ErrorCode.InternalServerError, "Ошибка при добавлении подписки на пользователя");
            }

            return Result.Success();
        }
    }
}