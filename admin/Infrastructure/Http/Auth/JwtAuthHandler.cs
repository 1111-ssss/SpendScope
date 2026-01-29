using admin.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace admin.Infrastructure.Http.Auth;

public class JwtAuthHandler : DelegatingHandler
{
    private readonly ITokenService _tokenService;

    public JwtAuthHandler(
        ITokenService tokenService
    )
    {
        _tokenService = tokenService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        if (request.RequestUri != null && IsPublicPath(request.RequestUri))
            return await base.SendAsync(request, cancellationToken);

        var token = await _tokenService.GetAccessTokenAsync(cancellationToken);

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await base.SendAsync(request, cancellationToken);

        //if (response.StatusCode == HttpStatusCode.Unauthorized)
        //{
        //    token = await _tokenService.GetAccessTokenAsync(cancellationToken);
        //    if (string.IsNullOrEmpty(token))
        //    {
        //        return response;
        //    }

        //    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //    response = await base.SendAsync(request, cancellationToken);
        //}

        return response;
    }

    private static readonly string[] _publicPaths =
        [
            "/auth"
        ];
    private bool IsPublicPath(Uri? uri)
    {
        if (uri == null)
            return true;

        string[] uriSegments = uri.Segments;

        if (_publicPaths.Any(p => uriSegments.Contains(p)))
            return true;

        return false;
    }
}