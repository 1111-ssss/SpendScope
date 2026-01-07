using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Features.Follows.GetFollowers;
using Application.Features.Follows.GetFollowing;
using Application.Features.Follows.FollowUser;
using Application.Features.Follows.UnFollowUser;
using Web.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Web.Controllers;

[ApiController]
[Route("api/follows")]
[Tags("Подписки")]
[Authorize]
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

        return result.ToActionResult();
    }
    [HttpGet("{userId}/following")]
    public async Task<IActionResult> GetFollowing(int userId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetFollowingQuery(userId), ct);

        return result.ToActionResult();
    }
    [HttpPost("{userId}")]
    public async Task<IActionResult> Follow(int userId, CancellationToken ct)
    {
        var result = await _mediator.Send(new FollowUserCommand(userId), ct);

        return result.ToActionResult();
    }
    [HttpDelete("{userId}")]
    public async Task<IActionResult> Unfollow(int userId, CancellationToken ct)
    {
        var result = await _mediator.Send(new UnFollowUserCommand(userId), ct);

        return result.ToActionResult();
    }
}