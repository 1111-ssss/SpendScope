using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Features.Auth.Login;
using Application.Features.Auth.Register;
using Web.Extensions.DependencyInjection;
using Microsoft.AspNetCore.RateLimiting;
using Application.Features.Auth.Refresh;

namespace Web.Controllers;

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

        return result.ToActionResult();
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);

        return result.ToActionResult();
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);

        return result.ToActionResult();
    }
}