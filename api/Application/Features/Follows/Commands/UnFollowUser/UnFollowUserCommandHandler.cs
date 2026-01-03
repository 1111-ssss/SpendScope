using Application.Abstractions.Auth;
using Application.Abstractions.DataBase;
using Application.Abstractions.Repository;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications.Follows;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Follows.UnFollowUser
{
    public class UnFollowUserCommandHandler : IRequestHandler<UnFollowUserCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        private readonly IBaseRepository<Follow> _followRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<UnFollowUserCommandHandler> _logger;
        public UnFollowUserCommandHandler(
            IUnitOfWork uow,
            IBaseRepository<Follow> followRepository,
            ICurrentUserService currentUserService,
            ILogger<UnFollowUserCommandHandler> logger)
        {
            _uow = uow;
            _followRepository = followRepository;
            _currentUserService = currentUserService;
            _logger = logger;
        }
        public async Task<Result> Handle(UnFollowUserCommand request, CancellationToken ct)
        {
            var userId = _currentUserService.GetUserId();
            if (userId == null)
                return Result.Failed(ErrorCode.Unauthorized, "Не удалось определить пользователя");

            await _followRepository.DeleteRangeAsync( new FollowExistsSpec(userId.Value, request.UserId), ct);

            try
            {
                await _uow.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении подписки пользователя");
                return Result.Failed(ErrorCode.InternalServerError, "Ошибка при удалении подписки пользователя");
            }

            return Result.Success();
        }
    }
}