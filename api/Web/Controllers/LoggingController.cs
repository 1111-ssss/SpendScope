using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Features.Profiles.GetProfile;
using Application.Features.Profiles.UpdateProfile;
using Application.Features.Profiles.GetAvatar;
using Application.Features.Profiles.DeleteAvatar;
using Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Application.Features.Logging.GetLogs;

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
}