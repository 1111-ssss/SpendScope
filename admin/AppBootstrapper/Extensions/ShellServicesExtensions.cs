using admin.Core.Interfaces;
using admin.Infrastructure.Services;
using admin.Shell;
using admin.Shell.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Wpf.Ui;
using Wpf.Ui.DependencyInjection;

namespace admin.AppBootstrapper.Extensions;

public static class ShellServicesExtensions
{
    public static IServiceCollection AddShellServices(this IServiceCollection services)
    {
        services.AddNavigationViewPageProvider();

        //App Host
        services.AddHostedService<ApplicationHostService>();

        //Shell services
        services.AddSingleton<IThemeService, ThemeService>();
        services.AddSingleton<ITaskBarService, TaskBarService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IContentDialogService, ContentDialogService>();

        //Shell
        services.AddSingleton<INavigationWindow, MainWindowView>();
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<IMainWindowController, MainWindowController>();

        return services;
    }
}
