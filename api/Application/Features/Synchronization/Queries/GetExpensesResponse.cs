using Domain.Entities;
using Microsoft.AspNetCore.StaticAssets;

namespace Application.Features.Synchronization.GetExpenses;

public class ExpenseDTO
{
    public int LocalId;
    public float Amount;
    public DateTime DateTime;
    public int CategoryId;
    public string Note;
    public ExpenseDTO(int localId, float amount, DateTime dateTime, int categoryId, string note)
    {
        LocalId = localId;
        Amount = amount;
        DateTime = dateTime;
        CategoryId = categoryId;
        Note = note;
    }

    public static ExpenseDTO FromEntity(Expense expense)
    {
        return new ExpenseDTO(expense.LocalId, expense.Amount, expense.DateTime, expense.CategoryId.Value, expense.Note ?? "");
    }
};
public record GetExpensesResponse(ExpenseDTO[] Expenses, int TotalCount);