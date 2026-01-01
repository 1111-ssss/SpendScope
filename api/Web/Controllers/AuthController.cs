// using Microsoft.AspNetCore.Mvc;
// using Application.Service.Auth.Handlers;
// using Application.DTO.Auth;

// [ApiController]
// [Route("api/[controller]")]
// [Tags("Аутентификация и авторизация")]
// public class AuthController : ControllerBase
// {
//     private readonly RegisterUserHandler _registerHandler;
//     private readonly LoginUserHandler _loginHandler;

//     public AuthController(RegisterUserHandler registerHandler, LoginUserHandler loginHandler)
//     {
//         _registerHandler = registerHandler;
//         _loginHandler = loginHandler;
//     }

//     /// <summary>
//     /// Регистрация нового пользователя
//     /// </summary>
//     [HttpPost("register")]
//     public async Task<IActionResult> Register([FromBody] RegisterUserRequest request, CancellationToken ct)
//     {
//         var result = await _registerHandler.Handle(request, ct);

//         if (result.IsSuccess)
//         {
//             var jwt = result.Value;

//             var cookieOptions = new CookieOptions
//             {
//                 HttpOnly = true,
//                 Secure = true,
//                 SameSite = SameSiteMode.Lax,
//                 Expires = DateTimeOffset.UtcNow.AddDays(7)
//             };

//             Response.Cookies.Append("jwt", jwt, cookieOptions);

//             return Ok(jwt);
//         }

//         return result.ToActionResult();
//     }

//     /// <summary>
//     /// Авторизация пользователя
//     /// </summary>
//     [HttpPost("login")]
//     public async Task<IActionResult> Login([FromBody] LoginUserRequest request, CancellationToken ct)
//     {
//         var result = await _loginHandler.Handle(request, ct);

//         if (result.IsSuccess)
//         {
//             var jwt = result.Value;

//             var cookieOptions = new CookieOptions
//             {
//                 HttpOnly = true,
//                 Secure = true,
//                 SameSite = SameSiteMode.Lax,
//                 Expires = DateTimeOffset.UtcNow.AddDays(7)
//             };

//             Response.Cookies.Append("jwt", jwt, cookieOptions);

//             return Ok(jwt);
//         }

//         return result.ToActionResult();
//     }
// }

using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Features.Auth.Login;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [Tags("Аутентификация и авторизация")]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginUserCommand command, CancellationToken ct)
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