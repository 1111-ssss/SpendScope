using System.Security.Claims;
using Application.DTO.AppVersion;
using Application.Service.Versions.Handlers;
using Domain.Entities;
using Domain.ValueObjects;
using Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Tags("Версии приложения")]
public class AppVersionController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly GetLatestHandler _latestHandler;
    private readonly UploadApkHandler _uploadHandler;
    private readonly string _apkPath;
    public AppVersionController(GetLatestHandler latestHandler, UploadApkHandler uploadHandler, IConfiguration config)
    {
        _latestHandler = latestHandler;
        _uploadHandler = uploadHandler;
        _config = config;
        _apkPath = _config.GetValue<string>("AppStorage:ApkPath") ?? "";
        if (!Directory.Exists(_apkPath))
            throw new ArgumentException("Путь к APK не указан в конфигурации");
    }
    [HttpGet]
    public async Task<IActionResult> GetLatest(
        [FromQuery] GetLatestVersionRequest request
        )
    {
        var result = await _latestHandler.Handle(request);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return result.ToActionResult();
    }
    [HttpGet("download/apk/{branch}/{build}")]
    public IActionResult DownloadApk(string branch, string build)
    {
        var safeBranch = Path.GetFileName(branch);
        var safeBuild = Path.GetFileName(build);
        if (string.IsNullOrWhiteSpace(safeBranch) || string.IsNullOrWhiteSpace(safeBuild))
            return BadRequest("Недопустимые параметры");
        var filePath = Path.Combine(_apkPath, safeBranch, safeBuild, "SpendScope.apk");
        var fullFilePath = Path.GetFullPath(filePath);
        var baseDir = Path.GetFullPath(_apkPath);
        if (!fullFilePath.StartsWith(baseDir, StringComparison.OrdinalIgnoreCase))
            return BadRequest("Недопустимый путь");
        if (!System.IO.File.Exists(fullFilePath))
            return NotFound("Файл не найден");
        return PhysicalFile(fullFilePath, "application/vnd.android.package-archive", "SpendScope.apk");
    }
    [HttpPost("upload")]
    [Authorize(Policy = "AdminOnly")]
    [Consumes("multipart/form-data")]
    [RequestFormLimits(MultipartBodyLengthLimit = 512_000_000)]
    [RequestSizeLimit(512_000_000)]
    public async Task<IActionResult> UploadApk(
        [FromForm] string branch,
        [FromForm] string build,
        [FromForm] string? changelog,
        IFormFile file)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString))
        {
            return Unauthorized("Не удалось определить пользователя");
        }
        var userId = new EntityId<User>(int.Parse(userIdString));

        var result = await _uploadHandler.Handle(
            new UploadVersionRequest(branch, build, userId, changelog), file, _apkPath);

        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return result.ToActionResult();
    }
}