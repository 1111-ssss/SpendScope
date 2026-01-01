using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Abstractions.Auth;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.Auth
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public int? UserId => int.TryParse(
            _httpContextAccessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.NameId)?.Value,
            out var id
        ) ? id : null;
        public bool IsAdmin => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value == "Admin";
    }
}