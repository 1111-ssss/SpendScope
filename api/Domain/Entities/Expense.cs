using Domain.Abstractions.Interfaces;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Expense : IAggregateRoot
{
    public EntityId<Expense> Id { get; private set; }
    public EntityId<User> UserId { get; private set; }
    public int LocalId { get; private set; }
    public float Amount { get; private set; }
    public DateTime DateTime { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public string? Note { get; private set; } = default!;
    public EntityId<Category> CategoryId { get; private set; }
    public bool IsDeleted { get; private set; } = false;
    private Expense() { }
    public static Expense Create(float amount, EntityId<User> userId, int localId, EntityId<Category> categoryId, DateTime dateTime, string? note = null)
    {
        return new Expense
        {
            LocalId = localId,
            Amount = amount,
            DateTime = dateTime,
            UserId = userId,
            Note = note,
            CategoryId = categoryId
        };
    }
    public void Update(float? amount, EntityId<Category>? categoryId, DateTime? dateTime, string? note = null)
    {
        Amount = amount ?? Amount;
        CategoryId = categoryId ?? CategoryId;
        DateTime = dateTime ?? DateTime;
        Note = note ?? Note;
        UpdatedAt = DateTime.UtcNow;
    }
}