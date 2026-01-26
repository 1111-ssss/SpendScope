using Microsoft.AspNetCore.Mvc;
using MediatR; 
using Web.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Application.Features.Health.GetHealth;
using Application.Features.Health.GetRequestCount;

namespace Web.Controllers;

[ApiController]
[Route("api/health")]
[Tags("Состояние")]
[Authorize]
[ApiVersion("1.0")]
public class HealthController : ControllerBase
{
    private readonly IMediator _mediator;
    public HealthController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetHealth(CancellationToken ct)
    {
        return Ok(); 
    }
    [HttpGet("ping")]
    public async Task<IActionResult> GetPing(CancellationToken ct)
    {
        return Ok(DateTime.UtcNow);
    }
    [HttpGet("requests")]
    public async Task<IActionResult> GetRequestCount([FromQuery] GetRequestCountQuery query, CancellationToken ct)
    {
        var result = await _mediator.Send(query, ct);

        return result.ToActionResult(); 
    }
    [HttpGet("detailed")]
    public async Task<IActionResult> GetDetailedHealth(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetHealthQuery(), ct);

        return result.ToActionResult();
    }
}