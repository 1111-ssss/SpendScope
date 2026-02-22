using admin.Features.Auth.Pages;
using admin.Features.Home;
using admin.Features.Metrics;
using admin.Features.Profile;
using admin.Features.Settings;
using admin.Features.Users;
using admin.Features.Versions;
using Microsoft.Extensions.DependencyInjection;

namespace admin.AppBootstrapper.Extensions;

public static class VMsAndPagesExtensions
{
    public static IServiceCollection AddVMsAndPages(this IServiceCollection services)
    {
        //Auth
        services.AddSingleton<AuthViewModel>();
        services.AddTransient<AuthLoginPage>();
        services.AddTransient<AuthRegisterPage>();
        services.AddTransient<AuthSettingsPage>();

        // ViewModels
        services.AddSingleton<HomeViewModel>();
        services.AddSingleton<ProfileViewModel>();
        services.AddSingleton<UsersViewModel>();
        services.AddSingleton<VersionsViewModel>();
        services.AddSingleton<MetricsViewModel>();
        services.AddSingleton<SettingsViewModel>();

        // Pages
        services.AddSingleton<HomePage>();
        services.AddSingleton<ProfilePage>();
        services.AddSingleton<UsersPage>();
        services.AddSingleton<VersionsPage>();
        services.AddSingleton<MetricsPage>();
        services.AddSingleton<SettingsPage>();

        return services;
    }
}
