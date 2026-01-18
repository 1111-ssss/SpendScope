using admin.Core.Interfaces;
using admin.Features.Auth;
using admin.Features.Home;
using admin.Shell;
using admin.Shell.ViewModel;
using admin.Shell.Views;
using Microsoft.Extensions.Hosting;
using System.Windows;
using Wpf.Ui;
using Microsoft.Extensions.DependencyInjection;

namespace admin.Infrastructure.Services;
public class ApplicationHostService(IServiceProvider serviceProvider) : IHostedService
{
    private INavigationWindow? _navigationWindow;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await HandleActivationAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }

    private async Task HandleActivationAsync()
    {
        await Task.CompletedTask;

        if (!Application.Current.Windows.OfType<MainWindowView>().Any())
        {
            _navigationWindow = (serviceProvider.GetService(typeof(INavigationWindow)) as INavigationWindow)!;
            _navigationWindow!.ShowWindow();
        }

        await Task.CompletedTask;
    }
}
