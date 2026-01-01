using Microsoft.AspNetCore.Mvc;
using Application.Service.Auth.Handlers;
using Application.DTO.Auth;
using Application.Service.Profiles.Handlers;
using Application.DTO.Profile;
using System.Security.Claims;
using Domain.ValueObjects;
using Domain.Entities;
using Application.Service.Follows.Handlers;

[ApiController]
[Route("api/[controller]")]
[Tags("Подписки")]
public class FollowController : ControllerBase
{
    private readonly GetFollowsHandler _getFollows;
    private readonly FollowHandler _follow;
    public FollowController(GetFollowsHandler getFollowsHandler, FollowHandler followHandler)
    {
        _follow = followHandler;
        _getFollows = getFollowsHandler;
    }

    /// <summary>
    /// Получение подписок пользователя
    /// </summary>
    [HttpGet("getFollowers")]
    public async Task<IActionResult> GetFollowers([FromQuery] int userId, CancellationToken ct)
    {
        var id = new EntityId<User>(userId);
        var result = await _getFollows.GetFollowers(id, ct);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return result.ToActionResult();
    }
    /// <summary>
    /// Получение подписок пользователя
    /// </summary>
    [HttpGet("getFollowing")]
    public async Task<IActionResult> GetFollowing([FromQuery] int userId, CancellationToken ct)
    {
        var id = new EntityId<User>(userId);
        var result = await _getFollows.GetFollowing(id, ct);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return result.ToActionResult();
    }
    /// <summary>
    /// Подписка на пользователя
    /// </summary>
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