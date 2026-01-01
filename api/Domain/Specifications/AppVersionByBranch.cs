// using Ardalis.Specification;
// using Domain.Entities;

// namespace Domain.Specifications
// {
//     public class AppVersionByBranchSpecification : Specification<AppVersion>
//     {
//         public AppVersionByBranchSpecification(string branch)
//         {
//             Query
//                 .Where(v => v.Branch == branch)
//                 .OrderByDescending(v => v.Build)
//                 .Take(1);
//         }
//     }
// }