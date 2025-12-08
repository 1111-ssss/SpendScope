using Application.DTO.AppVersion;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AppVersionController : ControllerBase
{
    private readonly IAppDbContext _db;
    private readonly IWebHostEnvironment _env;
    private readonly IConfiguration _config;
    public AppVersionController(IAppDbContext db, IWebHostEnvironment env, IConfiguration config)
    {
        _db = db;
        _env = env;
        _config = config;
    }
    [HttpGet]
    public async Task<ActionResult<GetLatestVersionResponse>> GetLatest(
        [FromQuery] GetLatestVersionRequest request
        )
    {
        var latest = await _db.AppVersions
            .Where(v => v.Branch == request.Branch)
            .OrderByDescending(v => v.Build)
            .Include(v => v.UploadedByNavigation)
            .FirstOrDefaultAsync();

        if (latest == null) return NotFound();

        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var downloadUrl = $"{baseUrl}/api/download/apk/{latest.Branch}/{latest.Build}/SpendScope.apk";

        return Ok(new GetLatestVersionResponse(
            Build: latest.Build,
            DownloadUrl: downloadUrl,
            Changelog: latest.Changelog,
            UploadedAt: latest.UploadedAt,
            UploadedBy: latest.UploadedByNavigation?.Username ?? "Unknown"
        ));
    }
    [HttpGet("api/download/apk/{**fileName}")]
    public IActionResult Download(string fileName)
    {
        var path = Path.Combine(
            Directory.GetCurrentDirectory(), "Files", "apk", fileName);

        if (!System.IO.File.Exists(path))
            return NotFound($"Файл не найден: {fileName}");

        var stream = System.IO.File.OpenRead(path);
        return File(stream, "application/vnd.android.package-archive", $"SpendScope.apk");
    }
}