using Microsoft.AspNetCore.Mvc;
using Application.Service.Auth;
using Application.DTO.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly RegisterUserHandler _registerHandler;

    public AuthController(RegisterUserHandler registerHandler)
    {
        _registerHandler = registerHandler;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        => (await _registerHandler.Handle(request)).ToActionResult();
}