using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.AppVersions
{
    public class AppVersionExistsSpec : Specification<AppVersion>
    {
        public AppVersionExistsSpec(string branch, int build)
        {
            Query
                .Where(v => v.Branch == branch && v.Build == build)
                .Take(1);
        }
    }
}