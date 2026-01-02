using MediatR;
using Domain.Abstractions.Result;
using Application.Common.Responses;

namespace Application.Features.Profiles.GetAvatar
{
    public record GetAvatarQuery(int UserId) : IRequest<Result<FileDownloadResponse>>;
}