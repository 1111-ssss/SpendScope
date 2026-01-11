using Ardalis.Specification;
using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Specifications.Sync;

public class ExpensesWithUserIdSpec : Specification<Expense>
{
    public ExpensesWithUserIdSpec(EntityId<User> userId)
    {
        Query
            .Where(e => e.UserId == userId);
    }
}