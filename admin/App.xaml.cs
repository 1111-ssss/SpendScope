using admin.AppBootstrapper;
using Microsoft.Extensions.Hosting;
using System.Windows;
using System.Windows.Threading;

namespace admin;

public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        _host = ApplicationBootstrapper.CreateHost();
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