using Application.Abstractions.Auth;
using Application.Abstractions.DataBase;
using Application.Abstractions.Repository;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications.Sync;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Synchronization.Sync;

public class SyncExpensesCommandHandler : IRequestHandler<SyncExpensesCommand, Result<SyncExpensesResponse>>
{
    private readonly IUnitOfWork _uow;
    private readonly IBaseRepository<Expense> _expenseRepository;
    private readonly ILogger<SyncExpensesCommandHandler> _logger;
    private readonly ICurrentUserService _currentUserService;
    public SyncExpensesCommandHandler(
        IUnitOfWork uow,
        IBaseRepository<Expense> expenseRepository,
        ILogger<SyncExpensesCommandHandler> logger,
        ICurrentUserService currentUserService
    )
    {
        _uow = uow;
        _expenseRepository = expenseRepository;
        _logger = logger;
        _currentUserService = currentUserService;
    }
    public async Task<Result<SyncExpensesResponse>> Handle(SyncExpensesCommand request, CancellationToken ct)
    {
        var userId = _currentUserService.GetUserId();
        if (userId == null)
            return Result<SyncExpensesResponse>.Failed(ErrorCode.Unauthorized, "Не удалось определить пользователя");

        var existingExpenses = await _expenseRepository.ListAsync(new ExpensesWithUserIdSpec(userId.Value), ct);

        var existingDict = existingExpenses.ToDictionary(x => x.LocalId);
        var newExpenses = new List<Expense>();

        foreach (var dto in request.Expenses)
        {
            if (existingDict.TryGetValue(dto.LocalId, out var existing))
            {
                existing.Update(dto.Amount, dto.CategoryId, dto.DateTime, dto.Note);
            }
            else
            {
                var newExpense = Expense.Create(dto.Amount, (EntityId<User>)userId, dto.LocalId, dto.CategoryId, dto.DateTime, dto.Note);
                newExpenses.Add(newExpense);
                existingDict[dto.LocalId] = newExpense;
            }
        }

        if (newExpenses.Count > 0)
        {
            await _expenseRepository.AddRangeAsync(newExpenses, ct);
        }

        var sorted = existingDict.Values
            .OrderByDescending(e => e.DateTime)
            .ThenByDescending(e => e.LocalId)
            .ToList();

        var toKeep = sorted.Take(100).ToList();
        var toRemove = sorted.Skip(100).ToList();

        if (toRemove.Count > 0)
        {
            await _expenseRepository.DeleteRangeAsync(toRemove, ct);
        }

        try
        {
            await _uow.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при синхронизации данных");
            return Result<SyncExpensesResponse>.Failed(ErrorCode.InternalServerError, $"Ошибка при синхронизации данных");
        }
        
        return Result<SyncExpensesResponse>.Success(new SyncExpensesResponse(
            TotalCount: sorted.Count,
            Kept: toKeep.Count,
            Removed: toRemove.Count
        ));
    }
}