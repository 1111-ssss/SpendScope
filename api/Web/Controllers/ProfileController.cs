using Microsoft.AspNetCore.Mvc;
using Application.Service.Auth.Handlers;
using Application.DTO.Auth;
using Application.Service.Profiles.Handlers;
using Application.DTO.Profile;
using System.Security.Claims;
using Domain.ValueObjects;
using Domain.Entities;

[ApiController]
[Route("api/[controller]")]
[Tags("Профиль пользователя")]
public class ProfileController : ControllerBase
{
    private readonly GetProfileHandler _getProfileHandler;
    private readonly UpdateProfileHandler _updateProfileHandler;
    private readonly UpdateAvatarHandler _updateAvatarHandler;
    private readonly string _avatarPath;

    public ProfileController(IConfiguration config, GetProfileHandler getProfileHandler, UpdateProfileHandler updateProfileHandler, UpdateAvatarHandler updateAvatarHandler)
    {
        _updateAvatarHandler = updateAvatarHandler;
        _getProfileHandler = getProfileHandler;
        _updateProfileHandler = updateProfileHandler;
        _avatarPath = config.GetValue<string>("AppStorage:AvatarPath") ?? "";
        if (!Directory.Exists(_avatarPath))
            throw new ArgumentException("Путь к аватарам не указан в конфигурации");
    }

    /// <summary>
    /// Получение профиля пользователя
    /// </summary>
    [HttpGet("get/{userId}")]
    public async Task<IActionResult> Get(int userId)
    {
        var id = new EntityId<User>(userId);
        var result = await _getProfileHandler.Handle(id);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return result.ToActionResult();
    }

    /// <summary>
    /// Изменение профиля пользователя
    /// </summary>
    [HttpPost("update")]
    public async Task<IActionResult> Update([FromBody] UpdateProfileRequest request)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString))
        {
            return Unauthorized("Не удалось определить пользователя");
        }
        var userId = new EntityId<User>(int.Parse(userIdString));

        var result = await _updateProfileHandler.Handle(request, userId);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return result.ToActionResult();
    }

    [HttpPost("updateAvatar")]
    public async Task<IActionResult> UpdateAvatar(IFormFile file)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString))
        {
            return Unauthorized("Не удалось определить пользователя");
        }
        var userId = new EntityId<User>(int.Parse(userIdString));

        var result = await _updateAvatarHandler.Handle(userId, file, _avatarPath);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return result.ToActionResult();
    }
    [HttpPost("deleteAvatar")]
    public async Task<IActionResult> DeleteAvatar()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString))
        {
            return Unauthorized("Не удалось определить пользователя");
        }
        var userId = new EntityId<User>(int.Parse(userIdString));

        var result = await _updateAvatarHandler.DeleteAvatar(userId, _avatarPath);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return result.ToActionResult();
    }
    [HttpGet("getAvatar/{userId}")]
    public async Task<IActionResult> GetAvatar(int userId)
    {
        var fullFilePath = Path.Combine(_avatarPath, $"{userId}.png");
        if (!System.IO.File.Exists(fullFilePath))
        {
            var defaultAvatarPath = Path.Combine(_avatarPath, "default-avatar.png");
            if (System.IO.File.Exists(defaultAvatarPath))
                return PhysicalFile(defaultAvatarPath, "image/png");

            return NotFound();
        }

        return PhysicalFile(fullFilePath, "image/png");
    }
}