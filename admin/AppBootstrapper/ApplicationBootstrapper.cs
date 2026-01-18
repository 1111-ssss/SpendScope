using admin.Core.Interfaces;
using admin.Features.Auth;
using admin.Features.Auth.Pages;
using admin.Features.Home;
using admin.Features.Settings;
using admin.Infrastructure.Services;
using admin.Shell;
using admin.Shell.ViewModel;
using admin.Shell.Views;
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
                services.AddSingleton<MainContentView>();
                services.AddSingleton<MainContentViewModel>();
                services.AddSingleton<ShellLoadingView>();
                services.AddSingleton<ShellLoadingViewModel>();

                // ViewModels
                services.AddSingleton<HomeViewModel>();
                services.AddSingleton<SettingsViewModel>();
                //services.AddSingleton<AuthViewModel>();
                services.AddSingleton<LoginViewModel>();
                services.AddSingleton<RegisterViewModel>();
                services.AddSingleton<AuthSettingsViewModel>();

                // Views
                services.AddSingleton<HomeView>();
                services.AddSingleton<SettingsView>();
                services.AddSingleton<AuthView>();
                services.AddSingleton<LoginView>();
                services.AddSingleton<RegisterView>();
                services.AddSingleton<AuthSettingsView>();

                services.AddTransient<AuthViewModel>(sp =>
                {
                    var login = sp.GetRequiredService<LoginViewModel>();
                    var register = sp.GetRequiredService<RegisterViewModel>();
                    var settings = sp.GetRequiredService<AuthSettingsViewModel>();
                    var mainVm = sp.GetRequiredService<MainWindowViewModel>();

                    return new AuthViewModel(mainVm.OnLoginSuccess, login, register, settings);
                });
            })
            .Build();
    }
}
