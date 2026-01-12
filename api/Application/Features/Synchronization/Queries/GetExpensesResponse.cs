using Domain.Entities;

namespace Application.Features.Synchronization.GetExpenses;

public record GetExpensesResponse(Expense[] Expenses, int TotalCount);