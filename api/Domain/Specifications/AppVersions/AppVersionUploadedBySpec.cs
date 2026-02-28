using Ardalis.Specification;
using Domain.Entities;

namespace Domain.Specifications.AppVersions;

public class AppVersionUploadedBySpec : Specification<AppVersion>
{
    public AppVersionUploadedBySpec()
    {
        Query
            .Include(x => x.UploadedBy)
            .OrderBy(x => x.Build);
    }
}