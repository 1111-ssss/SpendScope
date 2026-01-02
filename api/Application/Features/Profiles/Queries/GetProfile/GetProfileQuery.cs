using MediatR;
using Domain.Abstractions.Result;
using Application.Features.Profiles.Common;

namespace Application.Features.Profiles.GetProfile
{
    public record GetProfileQuery(int UserId) : IRequest<Result<ProfileResponse>>;
}