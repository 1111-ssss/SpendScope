using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications
{
    public class UserByUsernameSpecification : Specification<User>
    {
        public UserByUsernameSpecification(string username)
        {
            Query.Where(x => x.Username == username);
        }
    }
}