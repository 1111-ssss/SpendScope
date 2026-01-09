using Domain.Abstractions.Result;
using Domain.Entities;
using MediatR;

namespace Application.Features.Synchronization.Sync;

public record SyncExpensesCommand(Expense[] Expenses) : IRequest<Result>;