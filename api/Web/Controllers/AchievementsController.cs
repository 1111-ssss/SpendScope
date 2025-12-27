using Microsoft.AspNetCore.Mvc;
using Application.Service.Auth.Handlers;
using Application.DTO.Auth;
using Application.Service.Profiles.Handlers;
using Application.DTO.Profile;
using System.Security.Claims;
using Domain.ValueObjects;
using Domain.Entities;
using Application.Service.Achievements.Handlers;
using Microsoft.AspNetCore.Authorization;
using Application.DTO.Achievement;
using Microsoft.AspNetCore.Components.Forms;

[ApiController]
[Route("api/[controller]")]
[Tags("Достижения")]
public class AchievementsController : ControllerBase
{
    private readonly GetAchievementHandle _getAchievement;
    private readonly AddAchievementHandle _addAchievement;
    private readonly AchievementIconHandle _achievementIcon;
    private readonly string _achPath;

    public AchievementsController(IConfiguration config, GetAchievementHandle getAchievementHandle, AddAchievementHandle addAchievementHandle, AchievementIconHandle achievementIconHandle)
    {
        _achievementIcon = achievementIconHandle;
        _addAchievement = addAchievementHandle;
        _getAchievement = getAchievementHandle;
        _achPath = config.GetValue<string>("AppStorage:AchievementsPath") ?? "";
        if (!Directory.Exists(_achPath))
            throw new ArgumentException("Путь к иконкам достижений не указан в конфигурации");
    }

    /// <summary>
    /// Получение иконки достижения
    /// </summary>
    [HttpGet("getIcon")]
    public async Task<IActionResult> GetIcon([FromQuery] int iconId)
    {
        var fullFilePath = Path.Combine(_achPath, $"{iconId}.png");
        Console.WriteLine(fullFilePath);
        if (!System.IO.File.Exists(fullFilePath))
        {
            var defaultAvatarPath = Path.Combine(_achPath, "default-icon.png");
            if (System.IO.File.Exists(defaultAvatarPath))
                return PhysicalFile(defaultAvatarPath, "image/png");
            return NotFound();
        }

        return PhysicalFile(fullFilePath, "image/png");
    }
    /// <summary>
    /// Получение информации о достижении
    /// </summary>
    [HttpGet("{achId}")]
    public async Task<IActionResult> Get(int achId)
    {
        var id = new EntityId<Achievement>(achId);
        var result = await _getAchievement.Handle(id);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return result.ToActionResult();
    }
    /// <summary>
    /// Добавление достижения
    /// </summary>
    [HttpPost("addAchivement")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> AddAchievement([FromBody] AddAchievementRequest achData)
    {
        var result = await _addAchievement.Handle(achData);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return result.ToActionResult();
    }
    /// <summary>
    /// Добавить иконку достижения
    /// </summary>
    [HttpPost("addAchievementIcon")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> AddAchievementIcon([FromBody] int achId, IFormFile file)
    {
        var id = new EntityId<Achievement>(achId);
        var result = await _achievementIcon.Handle(id, file, _achPath);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return result.ToActionResult();
    }
}