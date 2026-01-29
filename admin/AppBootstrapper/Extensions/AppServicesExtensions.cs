using admin.Core.Interfaces;
using admin.Infrastructure.Http.Auth;
using admin.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace admin.AppBootstrapper.Extensions;

public static class AppServicesExtencions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddSingleton<IStorageService, StorageService>();
        services.AddSingleton<ICurrentUserService, CurrentUserService>();
        services.AddSingleton<ITokenService, TokenService>();
        services.AddSingleton<IErrorHandler, ErrorHandler>();
        services.AddSingleton<IAppSettingsService, AppSettingsService>();

        return services;
    }
}
