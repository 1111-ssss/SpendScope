using Domain.Abstractions.Result;
using MediatR;

namespace Application.Features.Synchronization.GetExpenses;

public record GetExpensesQuery() : IRequest<Result<GetExpensesResponse>>;