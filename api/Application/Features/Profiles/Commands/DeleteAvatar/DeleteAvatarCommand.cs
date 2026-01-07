using MediatR;
using Domain.Abstractions.Result;
using Application.Features.Profiles.Common;

namespace Application.Features.Profiles.DeleteAvatar;

public record DeleteAvatarCommand(int UserId) : IRequest<Result<ProfileResponse>>;