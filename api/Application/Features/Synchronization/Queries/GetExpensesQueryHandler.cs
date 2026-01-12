using Application.Abstractions.Auth;
using Application.Abstractions.DataBase;
using Application.Abstractions.Repository;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications.Sync;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Synchronization.GetExpenses;

public class GetExpensesQueryHandler : IRequestHandler<GetExpensesQuery, Result<GetExpensesResponse>>
{
    private readonly IUnitOfWork _uow;
    private readonly IBaseRepository<Expense> _expenseRepository;
    private readonly ILogger<GetExpensesQueryHandler> _logger;
    private readonly ICurrentUserService _currentUserService;
    public GetExpensesQueryHandler(
        IUnitOfWork uow,
        IBaseRepository<Expense> expenseRepository,
        ILogger<GetExpensesQueryHandler> logger,
        ICurrentUserService currentUserService
    )
    {
        _uow = uow;
        _expenseRepository = expenseRepository;
        _logger = logger;
        _currentUserService = currentUserService;
    }
    public async Task<Result<GetExpensesResponse>> Handle(GetExpensesQuery request, CancellationToken ct)
    {
        var userId = _currentUserService.GetUserId();
        if (userId == null)
            return Result.Unauthorized();

        var expenses = await _expenseRepository.ListAsync(new ExpensesWithUserIdSpec(userId.Value), ct);
        
        return Result<GetExpensesResponse>.Success(new GetExpensesResponse(
            Expenses: expenses.ToArray(),
            TotalCount: expenses.Count
        ));
    }
}