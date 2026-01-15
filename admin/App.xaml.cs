using admin.MVVM.ViewModels;
using admin.MVVM.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace admin
{
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    // ViewModels
                    services.AddSingleton<MainViewModel>();
                    services.AddTransient<DashboardPageViewModel>();

                    // // Окна/страницы
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<DashboardPage>();

                    // Здесь позже добавим сервисы, Refit и т.д.
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            base.OnExit(e);
        }
    }
}