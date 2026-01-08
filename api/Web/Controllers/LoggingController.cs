using Microsoft.AspNetCore.Mvc;
using MediatR; 
using Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Application.Features.Logging.GetLogs;
using Application.Features.Logging.ClearLogs;

namespace Web.Controllers;

[ApiController]
[Route("api/logging")]
[Tags("Логирование")]
[Authorize(Policy = "AdminOnly")]
[ApiVersion("1.0")]
public class LoggingController : ControllerBase
{
    private readonly IMediator _mediator;
    public LoggingController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetLogsQuery query, CancellationToken ct)
    {
        var result = await _mediator.Send(query, ct);

        return result.ToActionResult();
    }
    [HttpDelete]
    public async Task<IActionResult> ClearLogs([FromBody] ClearLogsCommand? command, CancellationToken ct)
    {
        var result = await _mediator.Send(command ?? new ClearLogsCommand(), ct);

        return result.ToActionResult();
    }
}