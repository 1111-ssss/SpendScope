using MediatR;
using Domain.Abstractions.Result;
using Microsoft.AspNetCore.Http;

namespace Application.Features.AppVersions.UploadVersion;

public record UploadVersionCommand(string Branch, int Build, string? Changelog, IFormFile File) : IRequest<Result<AppVersionResponse>>;