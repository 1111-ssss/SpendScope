using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Domain.ValueObjects;
using Domain.Entities;
using Application.Service.Follows.Handlers;
using MediatR;
using Application.Features.Follows.GetFollowers;
using Application.Features.Follows.GetFollowing;

[ApiController]
[Route("api/follow")]
[Tags("Подписки")]
[ApiVersion("1.0")]
public class FollowController : ControllerBase
{
    private readonly IMediator _mediator;
    public FollowController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet("{userId}/followers")]
    public async Task<IActionResult> GetFollowers(int userId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetFollowersQuery(userId), ct);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return result.ToActionResult();
    }
    [HttpGet("{userId}/following")]
    public async Task<IActionResult> GetFollowing(int userId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetFollowingQuery(userId), ct);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return result.ToActionResult();
    }
    [HttpPost("users/{userToFollow}/follow")]
    public async Task<IActionResult> Follow(int userToFollow, CancellationToken ct)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString))
        {
            return Unauthorized("Не удалось определить пользователя");
        }
        var userId = new EntityId<User>(int.Parse(userIdString));


        var id = new EntityId<User>(userToFollow);
        var result = await _follow.FollowUser(userId, id, ct);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return result.ToActionResult();
    }
    /// <summary>
    /// Подписка на пользователя
    /// </summary>
    [HttpDelete("users/{userToFollow}/follow")]
    public async Task<IActionResult> Unfollow(int userToUnfollow, CancellationToken ct)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString))
        {
            return Unauthorized("Не удалось определить пользователя");
        }
        var userId = new EntityId<User>(int.Parse(userIdString));


        var id = new EntityId<User>(userToUnfollow);
        var result = await _follow.UnfollowUser(userId, id, ct);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return result.ToActionResult();
    }
}