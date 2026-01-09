using Microsoft.AspNetCore.Mvc;
using MediatR; 
using Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Application.Features.Synchronization.Sync;

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
    [HttpPost]
    public async Task<IActionResult> Sync([FromBody] SyncExpensesCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);

        return result.ToActionResult(); 
    }
}