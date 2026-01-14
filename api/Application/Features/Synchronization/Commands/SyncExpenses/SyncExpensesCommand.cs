using Domain.Abstractions.Result;
using MediatR;

namespace Application.Features.Synchronization.SyncExpenses;

public record ExpenseDTO(int LocalId, float Amount, DateTime DateTime, int CategoryId, string Note);
public record SyncExpensesCommand(ExpenseDTO[] Expenses) : IRequest<Result<SyncExpensesResponse>>;