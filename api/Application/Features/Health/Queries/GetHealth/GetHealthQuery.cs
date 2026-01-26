using Domain.Abstractions.Result;
using MediatR;

namespace Application.Features.Health.GetHealth;

public record GetHealthQuery : IRequest<Result<HealthResponse>>;