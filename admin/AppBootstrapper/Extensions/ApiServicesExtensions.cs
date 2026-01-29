using admin.Core.Interfaces;
using admin.Infrastructure.Http.Auth;
using admin.Infrastructure.Http.Clients;
using admin.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace admin.AppBootstrapper.Extensions;

public static class ApiServicesExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        //Api services
        services.AddSingleton<IApiService, ApiService>();
        services.AddSingleton<JwtAuthHandler>();

        //Refit
        services.AddHttpClient("NoAuthClient", client =>
        {
            client.BaseAddress = new("http://127.0.0.1:5012/api");
            client.Timeout = TimeSpan.FromSeconds(20);
        })
        //.AddHttpMessageHandler<JwtAuthHandler>()
        .AddTypedClient<IAuthApi>(Refit.RestService.For<IAuthApi>);

        services.AddHttpClient("AuthClient", client =>
        {
            client.BaseAddress = new("http://127.0.0.1:5012/api");
            client.Timeout = TimeSpan.FromSeconds(20);
        })
        .AddHttpMessageHandler<JwtAuthHandler>()
        .AddTypedClient<IHealthApi>(Refit.RestService.For<IHealthApi>)
        .AddTypedClient<IProfileApi>(Refit.RestService.For<IProfileApi>);

        return services;
    }
}
