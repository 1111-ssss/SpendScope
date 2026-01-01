using MediatR;
using Domain.Abstractions.Result;

namespace Application.Features.AppVersions.GetLatestVersion
{
    public record GetLatestVersionQuery(string Branch, int Build) : IRequest<Result<AppVersionResponse>>;
}