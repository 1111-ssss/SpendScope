using Application.Features.AppVersions.DownloadVersion;
using Application.Features.AppVersions.GetLatestVersion;
using Application.Features.AppVersions.UploadVersion;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/versions")]
[Authorize]
[Tags("Версии приложения")]
[ApiVersion("1.0")]
public class AppVersionController : ControllerBase
{
    private readonly IMediator _mediator;
    public AppVersionController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet]
    public async Task<IActionResult> GetLatest([FromQuery] GetLatestVersionQuery query, CancellationToken ct)
    {
        var result = await _mediator.Send(query, ct);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return result.ToActionResult();
    }
    [HttpGet("download")]
    public async Task<IActionResult> DownloadApk([FromQuery] DownloadVersionQuery query, CancellationToken ct)
    {
        var result = await _mediator.Send(query, ct);

        if (result.IsSuccess)
        {
            return PhysicalFile(result.Value.FilePath, result.Value.ContentType, result.Value.FileName);
        }

        return NotFound("Файл не найден");
    }
    [HttpPost("upload")]
    [Authorize(Policy = "AdminOnly")]
    [Consumes("multipart/form-data")]
    [RequestFormLimits(MultipartBodyLengthLimit = 512_000_000)]
    [RequestSizeLimit(512_000_000)]
    public async Task<IActionResult> UploadFile([FromForm] UploadVersionCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return result.ToActionResult();
    }
}