namespace Application.Features.Synchronization.SyncExpenses;

public record SyncExpensesResponse(int TotalCount, int Kept, int Removed);