using MediatR;
using Domain.Abstractions.Result;

namespace Application.Features.AppVersions.DeleteVersion;

public record DeleteVersionCommand(string Branch, int Build) : IRequest<Result>;