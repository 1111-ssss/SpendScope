using Microsoft.AspNetCore.Mvc;
using Application.Service.Auth;
using Application.DTO.Auth;

[ApiController]
[Route("api/[controller]")]
[Tags("Аутентификация и авторизация")]
public class AuthController : ControllerBase
{
    private readonly RegisterUserHandler _registerHandler;
    private readonly LoginUserHandler _loginHandler;

    public AuthController(RegisterUserHandler registerHandler, LoginUserHandler loginHandler)
    {
        _registerHandler = registerHandler;
        _loginHandler = loginHandler;
    }

    /// <summary>
    /// Регистрация нового пользователя
    /// </summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        var result = await _registerHandler.Handle(request);

        if (result.IsSuccess)
        {
            var jwt = result.Value;

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("jwt", jwt, cookieOptions);

            return Ok(jwt);
        }

        return result.ToActionResult();
    }

    /// <summary>
    /// Авторизация пользователя
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
    {
        var result = await _loginHandler.Handle(request);

        if (result.IsSuccess)
        {
            var jwt = result.Value;

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("jwt", jwt, cookieOptions);

            return Ok(jwt);
        }

        return result.ToActionResult();
    }
}