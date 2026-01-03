using Application.Features.Auth;
using Domain.Abstractions.Result;
using Domain.Entities;
using Application.Abstractions.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services.Auth;

public class JwtGenerator : IJwtGenerator
{
    private readonly IConfiguration _config;
    private readonly string _key;
    public JwtGenerator(IConfiguration config)
    {
        _config = config;
        _key = _config["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key пустой в конфигурации");
    }
    public Result<AuthResponse> GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };
        if (user.IsAdmin)
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                int.Parse(_config["Jwt:Expires"] ?? "15")
            ),
            audience: _config["Jwt:Audience"],
            issuer: _config["Jwt:Issuer"],
            signingCredentials: creds
        );

        return Result<AuthResponse>.Success(new AuthResponse
        {
            JwtToken = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = DateTime.UtcNow.AddMinutes(
                (token.ValidTo - token.ValidFrom).TotalMinutes
            )
        });
    }
}