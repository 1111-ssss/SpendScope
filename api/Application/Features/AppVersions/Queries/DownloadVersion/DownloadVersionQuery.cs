using Domain.Abstractions.Result;
using MediatR;

namespace Application.Features.AppVersions.DownloadVersion
{
    public record DownloadVersionQuery(string Branch, int Build) : IRequest<Result<DownloadVersionResponse>>;
}