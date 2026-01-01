using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.Auth
{
    public class UserByUsernameOrEmailSpec : Specification<User>
    {
        public UserByUsernameOrEmailSpec(string identifier)
        {
            Query.Where(e =>
                e.Username == identifier ||
                e.Email == identifier
            );
        }
        public UserByUsernameOrEmailSpec(string username, string email)
        {
            Query.Where(e =>
                e.Username == username ||
                e.Email == email
            );
        }
    }
}