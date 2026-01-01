using Application.Abstractions.Interfaces;
using Application.DTO.AppVersion;
using Application.DTO.Profile;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications;
using Domain.ValueObjects;
using Logger;

namespace Application.Service.Follows.Handlers
{
    public class FollowHandler
    {
        private readonly IUnitOfWork _db;
        private readonly IRepository<User> _users;
        private readonly IRepository<Follow> _follows;
        private readonly ICustomLogger<FollowHandler> _logger;
        public FollowHandler(ICustomLogger<FollowHandler> logger, IUnitOfWork db, IRepository<User> users, IRepository<Follow> follows)
        {
            _logger = logger;
            _follows = follows;
            _db = db;
            _users = users;
        }
        public async Task<Result<bool>> FollowUser(EntityId<User> userId, EntityId<User> userToFollow, CancellationToken ct = default)
        {
            await _follows.AddAsync(
                Follow.Create(userId, userToFollow),
                ct
            );
            try
            {
                await _db.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.Error("Ошибка при добавлении подписки на пользователя", ex);
                return Result<bool>.Failed(ErrorCode.InternalServerError, "Ошибка при добавлении подписки на пользователя");
            }

            return Result<bool>.Success(true);
        }
        public async Task<Result<bool>> UnfollowUser(EntityId<User> userId, EntityId<User> userToUnfollow, CancellationToken ct = default)
        {
            await _follows.DeleteWhereAsync(new FollowExistsSpec(userToUnfollow, userId), ct);
            try
            {
                await _db.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.Error("Ошибка при удалении подписки на пользователя", ex);
                return Result<bool>.Failed(ErrorCode.InternalServerError, "Ошибка при удалении подписки на пользователя");
            }

            return Result<bool>.Success(true);
        }
    }
}