using Microsoft.AspNetCore.Mvc;
using MediatR; 
using Web.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;

namespace Web.Controllers;

[ApiController]
[Route("api/health")]
[Tags("Синхронизация")]
[Authorize]
[ApiVersion("1.0")]
public class HealthController : ControllerBase
{
    private readonly IMediator _mediator;
    public HealthController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet]
    public async Task<IActionResult> GetHealth(CancellationToken ct)
    {
        return Ok(); 
    }
    [HttpGet("ping")]
    public async Task<IActionResult> Ping(CancellationToken ct)
    {
        return Ok(); 
    }
}