using admin.Services;
using admin.ViewModels;
using admin.ViewModels.MainWindow;
using admin.Views;
using admin.Views.Pages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using Wpf.Ui;
using Wpf.Ui.DependencyInjection;

namespace admin;

public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
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

                //Main window with navigation
                services.AddSingleton<INavigationWindow, MainWindow>();

                // ViewModels
                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton<HomeViewModel>();

                // Views
                services.AddSingleton<HomeView>();
                services.AddSingleton<SettingsView>();
            })
            .Build();
    }

    private async void OnStartup(object sender, StartupEventArgs e)
    {
        await _host.StartAsync();

        //base.OnStartup(e)
    }
    private async void OnExit(object sender, ExitEventArgs e)
    {
        await _host.StopAsync();

        //base.OnExit(e);
        _host.Dispose();
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        
    }
}