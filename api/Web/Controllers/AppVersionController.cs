using Application.Features.AppVersions.DeleteVersion;
using Application.Features.AppVersions.DownloadVersion;
using Application.Features.AppVersions.GetLatestVersion;
using Application.Features.AppVersions.UploadVersion;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Web.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Application.Features.AppVersions.GetAllVersions;

namespace Web.Controllers;

[ApiController]
[Route("api/versions")]
[Authorize]
[Tags("Версии приложения")]
[EnableRateLimiting("DefaultLimiter")]
[ApiVersion("1.0")]
public class AppVersionController : ControllerBase
{
    private readonly IMediator _mediator;
    public AppVersionController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet("{branch}")]
    public async Task<IActionResult> GetLatest(string branch, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetLatestVersionQuery(branch), ct);

        return result.ToActionResult();
    }
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllVersionsQuery(), ct);

        return result.ToActionResult();
    }
    [HttpGet("{branch}/{build}")]
    public async Task<IActionResult> DownloadApk(string branch, int build, [FromQuery] string fileType, CancellationToken ct)
    {
        var result = await _mediator.Send(new DownloadVersionQuery(branch, build, fileType), ct);

        if (result.IsSuccess)
        {
            return PhysicalFile(result.Value.FilePath, result.Value.ContentType, result.Value.FileName);
        }

        return result.ToActionResult();
    }
    [HttpPost("upload")]
    [Authorize(Policy = "AdminOnly")]
    [Consumes("multipart/form-data")]
    [RequestFormLimits(MultipartBodyLengthLimit = 512_000_000)]
    [RequestSizeLimit(512_000_000)]
    public async Task<IActionResult> UploadFile([FromForm] UploadVersionCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);

        return result.ToActionResult();
    }
    [HttpDelete("{branch}/{build}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteVersion(string branch, int build, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteVersionCommand(branch, build), ct);

        return result.ToActionResult();
    }
}