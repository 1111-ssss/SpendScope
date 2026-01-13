using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Application.Features.Achievements.AchievementInfo;
using Application.Features.Achievements.AchievementIcon;
using Application.Features.Achievements.AddAchievement;
using Application.Features.Achievements.UpdateAchievement;
using Web.Extensions.DependencyInjection;

namespace Web.Controllers;

[ApiController]
[Route("api/achievements")]
[Tags("Достижения")]
[Authorize]
[ApiVersion("1.0")]
public class AchievementsController : ControllerBase
{
    private readonly IMediator _mediator;
    public AchievementsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet("{achId}/icon")]
    public async Task<IActionResult> GetIcon(int achId, CancellationToken ct)
    {
        var result = await _mediator.Send(new AchievementIconQuery(achId), ct);

        if (result.IsSuccess)
        {
            return PhysicalFile(result.Value.FilePath, result.Value.ContentType);
        }

        return result.ToActionResult();
    }
    [HttpGet("{achId}")]
    public async Task<IActionResult> Get(int achId, CancellationToken ct)
    {
        var result = await _mediator.Send(new AchievementInfoQuery(achId), ct);

        return result.ToActionResult();
    }
    [HttpPost("add")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> AddAchievement([FromForm] AddAchievementCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);

        return result.ToActionResult();
    }
    [HttpPatch("update")]
    [Authorize(Policy = "AdminOnly")]
    [RequestFormLimits(MultipartBodyLengthLimit = 8_000_000)]
    [RequestSizeLimit(8_000_000)]
    public async Task<IActionResult> UpdateAchievement([FromForm] UpdateAchievementCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);

        return result.ToActionResult();
    }
}