using admin.Core.Interfaces;
using admin.Features.Auth.Pages;
using admin.Features.Home;
using admin.Features.Settings;
using admin.Infrastructure.Http.Auth;
using admin.Infrastructure.Http.Clients;
using admin.Infrastructure.Services;
using admin.Shell;
using admin.Shell.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

                //Logging
                services.AddLogging(conf =>
                {
                    conf.AddConsole();
                    conf.AddDebug();
                    conf.SetMinimumLevel(LogLevel.Debug);
                });

                //Services
                services.AddSingleton<INavigationService, NavigationService>();
                services.AddSingleton<IContentDialogService, ContentDialogService>();
                services.AddSingleton<IApiService, ApiService>();
                services.AddSingleton<IStorageService, StorageService>();
                services.AddSingleton<ICurrentUserService, CurrentUserService>();
                services.AddSingleton<ITokenService, TokenService>();
                services.AddSingleton<IErrorHandler, ErrorHandler>();
                services.AddSingleton<JwtAuthHandler>();

                //Refit
                services.AddHttpClient("NoAuthClient", client =>
                {
                    client.BaseAddress = new("http://127.0.0.1:5012/api");
                    client.Timeout = TimeSpan.FromSeconds(20);
                })
                //.AddHttpMessageHandler<JwtAuthHandler>()
                .AddTypedClient<IAuthApi>(Refit.RestService.For<IAuthApi>);

                //services.AddHttpClient("AuthClient", client =>
                //{
                //    client.BaseAddress = new("http://127.0.0.1:5012/api");
                //    client.Timeout = TimeSpan.FromSeconds(20);
                //})
                //.AddHttpMessageHandler<JwtAuthHandler>();

                //Shell
                services.AddSingleton<INavigationWindow, MainWindowView>();
                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton<IMainWindowController, MainWindowController>();

                //Auth
                services.AddSingleton<AuthViewModel>();
                services.AddTransient<AuthLoginPage>();
                services.AddTransient<AuthRegisterPage>();
                services.AddTransient<AuthSettingsPage>();

                // ViewModels
                services.AddSingleton<HomeViewModel>();
                services.AddSingleton<SettingsViewModel>();

                // Pages
                services.AddSingleton<HomePage>();
                services.AddSingleton<SettingsPage>();
            })
            .Build();
    }
}
