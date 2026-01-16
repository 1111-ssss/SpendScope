using admin.ViewModels;
using admin.Views;
using admin.Views.Pages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using Wpf.Ui.DependencyInjection;

namespace admin;

public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                _ = services.AddNavigationViewPageProvider();

                // ViewModels
                services.AddSingleton<MainWindowViewModel>();

                // Views
                services.AddSingleton<MainWindow>();
                services.AddSingleton<HomeView>();
                services.AddSingleton<SettingsView>();

                // Services
                //
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();

        //var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        //mainWindow.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync();
        base.OnExit(e);
    }
}