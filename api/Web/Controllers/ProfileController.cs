using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Features.Profiles.GetProfile;
using Application.Features.Profiles.UpdateProfile;
using Application.Features.Profiles.GetAvatar;
using Application.Features.Profiles.DeleteAvatar;
using Microsoft.AspNetCore.Authorization;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/profile")]
    [Tags("Профиль пользователя")]
    [Authorize]
    [ApiVersion("1.0")]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(int userId, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetProfileQuery(userId), ct);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return result.ToActionResult();
        }
        [HttpPatch("update")]
        public async Task<IActionResult> Update([FromForm] UpdateProfileCommand command, CancellationToken ct)
        {
            var result = await _mediator.Send(command, ct);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return result.ToActionResult();
        }
        [HttpGet("{userId}/avatar")]
        public async Task<IActionResult> GetAvatar(int userId, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetAvatarQuery(userId), ct);

            if (result.IsSuccess)
            {
                return PhysicalFile(result.Value.FilePath, result.Value.ContentType);
            }

            return NotFound("Файл не найден");
        }
        [HttpDelete("{userId}/avatar")]
        public async Task<IActionResult> DeleteAvatar(int userId, CancellationToken ct)
        {
            var result = await _mediator.Send(new DeleteAvatarCommand(userId), ct);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return result.ToActionResult();
        }
    }
}