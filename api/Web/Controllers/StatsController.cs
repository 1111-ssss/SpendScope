using Microsoft.AspNetCore.Mvc;
using MediatR; 
using Web.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Application.Features.Stats.GetRequestCount;

namespace Web.Controllers;

[ApiController]
[Route("api/stats")]
[Tags("Статистика")]
[Authorize]
[ApiVersion("1.0")]
public class StatsController : ControllerBase
{
    private readonly IMediator _mediator;
    public StatsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("requests")]
    public async Task<IActionResult> GetRequestCount([FromQuery] GetRequestCountQuery query, CancellationToken ct)
    {
        var result = await _mediator.Send(query, ct);

        return result.ToActionResult(); 
    }
}