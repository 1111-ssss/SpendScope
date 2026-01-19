using admin.AppBootstrapper;
using Microsoft.Extensions.DependencyInjection;
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
        //base.OnStartup(e);

        await _host.StartAsync();
    }
    private async void OnExit(object sender, ExitEventArgs e)
    {
        await _host.StopAsync();

        _host.Dispose();
        base.OnExit(e);
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        
    }
    public static T? GetService<T>() where T : class
    {
        return ((App)Current)._host.Services.GetService<T>();
    }
    public static T GetRequiredService<T>() where T : class
    {
        return ((App)Current)._host.Services.GetRequiredService<T>();
    }
}