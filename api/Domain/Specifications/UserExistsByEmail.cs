using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications
{
    public class UserByEmailSpecification : Specification<User>
    {
        public UserByEmailSpecification(string email)
        {
            Query.Where(x => x.Email == email);
        }
    }
}