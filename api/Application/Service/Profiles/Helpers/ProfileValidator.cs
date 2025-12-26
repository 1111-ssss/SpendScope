using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Domain.Abstractions.Result;

namespace Application.Service.Profiles.Helpers
{
    public class ProfileValidator
    {
        public const int MaxDisplayNameLength = 20;
        public const int MaxBioLength = 400;
        public ProfileValidator()
        {
        }
        public string? FormatString(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;
            var formatted = Regex.Replace(input.Trim(), @"\s+", " ");
            return formatted.Length > 0 ? formatted : null;
        }
        public bool IsValidLength(string? input, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(input))
                return true;
            return input.Trim().Length <= maxLength;
        }
        public bool HasValidCharactersDisplayName(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return true;
            return Regex.IsMatch(input, @"^[\p{L}\p{N}\s\-_.()'&]+$");
        }
        public bool HasValidCharactersBio(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return true;
            return Regex.IsMatch(input, @"^[\p{L}\p{N}\p{Zs}\p{P}\p{S}]+$");
        }
        public Result<string> ValidateDisplayName(string? input)
        {
            var formatted = FormatString(input);
            if (formatted == null)
                return Result<string>.Success("");
            if (formatted.Length > MaxDisplayNameLength)
                return Result<string>.Failed(ErrorCode.BadRequest, $"Имя не может быть длиннее {MaxDisplayNameLength} символов");
            if (!HasValidCharactersDisplayName(formatted))
                return Result<string>.Failed(ErrorCode.BadRequest, "Имя содержит недопустимые символы. Разрешены буквы, цифры, пробел, дефис, подчёркивание, точка, скобки и апостроф");
            return Result<string>.Success(formatted);
        }
        public Result<string> ValidateBio(string? input)
        {
            var formatted = FormatString(input);
            if (formatted == null)
                return Result<string>.Success("");
            if (formatted.Length > MaxBioLength)
                return Result<string>.Failed(ErrorCode.BadRequest, $"Биография не может быть длиннее {MaxBioLength} символов");
            if (!HasValidCharactersBio(formatted))
                return Result<string>.Failed(ErrorCode.BadRequest, "Биография содержит недопустимые символы");
            return Result<string>.Success(formatted);
        }
    }
}
