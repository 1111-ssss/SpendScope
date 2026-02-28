using Domain.Abstractions.Result;
using MediatR;

namespace Application.Features.AppVersions.GetAllVersions;

public record GetAllVersionsQuery : IRequest<Result<GetAllVersionsResponse>>;