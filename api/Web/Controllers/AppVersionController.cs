using Application.DTO.AppVersion;
using Application.Service.Versions.Handlers;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Tags("Версии приложения")]
public class AppVersionController : ControllerBase
{
    private readonly IWebHostEnvironment _env;
    private readonly IConfiguration _config;
    private readonly GetLatestHandler _handler;

    public AppVersionController(GetLatestHandler handler, IWebHostEnvironment env, IConfiguration config)
    {
        _handler = handler;
        _env = env;
        _config = config;
    }
    [HttpGet]
    public async Task<IActionResult> GetLatest(
        [FromQuery] GetLatestVersionRequest request
        )
    {
        var result = await _handler.Handle(request);
        return result.ToActionResult();
    }
    [HttpGet("download/apk/{**fileName}")]
    public IActionResult DownloadApk(string fileName)
    {
        var path = Path.Combine(
            Directory.GetCurrentDirectory(), "Files", "apk", fileName, "SpendScope.apk");

        if (!System.IO.File.Exists(path))
            return NotFound($"Файл не найден: {fileName}");

        var stream = System.IO.File.OpenRead(path);
        return File(stream, "application/vnd.android.package-archive", $"SpendScope.apk");
    }
}