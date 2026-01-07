using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.AppVersions;

public class AppVersionByBranchSpec : Specification<AppVersion>
{
    public AppVersionByBranchSpec(string branch)
    {
        Query
            .Where(v => v.Branch == branch)
            .OrderByDescending(v => v.Build)
            .Take(1);
    }
}