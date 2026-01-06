using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Features.Auth.Login;
using Application.Features.Auth.Register;
using Microsoft.AspNetCore.RateLimiting;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [Tags("Аутентификация и авторизация")]
    [EnableRateLimiting("DefaultLimiter")]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command, CancellationToken ct)
        {
            var result = await _mediator.Send(command, ct);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return result.ToActionResult();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command, CancellationToken ct)
        {
            var result = await _mediator.Send(command, ct);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return result.ToActionResult();
        }
    }
}