using Ardalis.Specification;

namespace Application.Abstractions.Repository;

public interface IBaseRepository<T> : IRepositoryBase<T> where T : class
{
    //тут можно добавить общие методы для всех репозиториев
    //но пока хватает и методов из IRepositoryBase
}