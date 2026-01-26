using Domain.Abstractions.Result;
using MediatR;

namespace Application.Features.Health.GetRequestCount;

public record GetRequestCountQuery(DateTime DateTime) : IRequest<Result<int>>;