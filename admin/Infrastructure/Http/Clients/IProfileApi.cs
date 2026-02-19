using admin.Core.DTO.Profiles.Responses;
using Refit;
using System.Net.Http;

namespace admin.Infrastructure.Http.Clients;

public interface IProfileApi
{
    [Get("/profile/{userId}")]
    Task<ProfileResponse> GetProfile([Query] int userId, CancellationToken ct = default);
    //update
    [Get("/profile/{userId}/avatar")]
    Task<HttpResponseMessage> GetAvatar(int userId, CancellationToken ct = default);
    //delete avatar
    [Get("/profile/search")]
    Task<ProfilesPageResponse> SearchProfiles([Query] string username, [Query] int page, [Query] int pageSize, CancellationToken ct = default);
}