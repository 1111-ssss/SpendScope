using Application.Abstractions.Interfaces;
using Domain.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T> : RepositoryBase<T> where T : class, IAggregateRoot
    {
        public GenericRepository(AppDbContext db) : base(db)
        {
        }
    }
}
