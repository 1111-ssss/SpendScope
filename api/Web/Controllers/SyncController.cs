using Microsoft.AspNetCore.Mvc;
using MediatR; 
using Web.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Application.Features.Synchronization.SyncExpenses;
using Microsoft.AspNetCore.RateLimiting;
using Application.Features.Synchronization.GetExpenses;

namespace Web.Controllers;

[ApiController]
[Route("api/sync")]
[Tags("Синхронизация")]
[Authorize]
[ApiVersion("1.0")]
public class SyncController : ControllerBase
{
    private readonly IMediator _mediator;
    public SyncController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [EnableRateLimiting("StrictLimiter")]
    [HttpPost]
    public async Task<IActionResult> Sync([FromBody] SyncExpensesCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);

        return result.ToActionResult(); 
    }
    [HttpGet]
    public async Task<IActionResult> GetExpenses(GetExpensesQuery query, CancellationToken ct)
    {
        var result = await _mediator.Send(query, ct);

        return result.ToActionResult(); 
    }
}