using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class LoginUserDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}
