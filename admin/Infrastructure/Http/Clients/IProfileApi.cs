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
}