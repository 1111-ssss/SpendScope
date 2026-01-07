using MediatR;
using Domain.Abstractions.Result;
using Application.Features.Profiles.Common;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Profiles.UpdateProfile;

public record UpdateProfileCommand(string? DisplayName, string? Bio, IFormFile? Image) : IRequest<Result<ProfileResponse>>;