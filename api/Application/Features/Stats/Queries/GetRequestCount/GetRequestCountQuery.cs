using Domain.Abstractions.Result;
using MediatR;

namespace Application.Features.Stats.GetRequestCount;

public record GetRequestCountQuery(DateTime DateTime) : IRequest<Result<int>>;