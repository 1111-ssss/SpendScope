using admin.Core.DTO.Common.Responses;
using admin.Core.DTO.Profiles.Responses;
using Refit;

namespace admin.Infrastructure.Http.Clients;

public interface IProfileApi
{
    [Get("/profile")]
    Task<ProfileResponse> GetProfile([Query] int userId, CancellationToken ct = default);
    //update
    [Get("/profile/{userId}/avatar")]
    Task<FileDownloadResponse> GetAvatar(int userId, CancellationToken ct = default);
    //delete avatar
}