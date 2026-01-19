using admin.Core.Interfaces;
using admin.Features.Auth.Pages;
using admin.Features.Home;
using admin.Features.Settings;
using admin.Infrastructure.Services;
using admin.Shell;
using admin.Shell.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using Wpf.Ui;
using Wpf.Ui.DependencyInjection;

namespace admin.AppBootstrapper;

public static class ApplicationBootstrapper
{
    public static IHost CreateHost()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(c =>
            {
                var basePath =
                    Path.GetDirectoryName(AppContext.BaseDirectory)
                    ?? throw new DirectoryNotFoundException(
                        "Директория приложения не найдена."
                    );
                _ = c.SetBasePath(basePath);
            })
            .ConfigureServices(services =>
            {
                services.AddNavigationViewPageProvider();

                //App Host
                services.AddHostedService<ApplicationHostService>();

                //Themes
                services.AddSingleton<IThemeService, ThemeService>();

                //Task bar
                services.AddSingleton<ITaskBarService, TaskBarService>();

                //Services
                services.AddSingleton<INavigationService, NavigationService>();
                services.AddSingleton<IAuthService, AuthService>();

                //Shell
                services.AddSingleton<INavigationWindow, MainWindowView>();
                services.AddSingleton<MainWindowViewModel>();

                //Auth
                services.AddSingleton<AuthViewModel>();
                services.AddTransient<AuthLoginPage>();
                services.AddTransient<AuthRegisterPage>();
                services.AddTransient<AuthSettingsPage>();

                // ViewModels
                services.AddSingleton<HomeViewModel>();
                services.AddSingleton<SettingsViewModel>();

                // Views
                services.AddSingleton<HomePage>();
                services.AddSingleton<SettingsPage>();
            })
            .Build();
    }
}
