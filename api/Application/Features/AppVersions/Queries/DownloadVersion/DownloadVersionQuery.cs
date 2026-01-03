using Application.Common.Responses;
using Domain.Abstractions.Result;
using MediatR;

namespace Application.Features.AppVersions.DownloadVersion
{
    public record DownloadVersionQuery(string Branch, int Build, string FileType) : IRequest<Result<FileDownloadResponse>>;
}