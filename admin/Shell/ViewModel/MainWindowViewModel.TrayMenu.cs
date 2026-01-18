using admin.Features.Home;
using admin.Features.Settings;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace admin.Shell.ViewModel;
public partial class MainWindowViewModel
{
    private static Dictionary<string, Type> _appPages = new() {
        { "tray_home", typeof(HomeView) },
        { "tray_settings", typeof(SettingsView) },
    };

    private void OnTrayMenuItemClick(object sender, RoutedEventArgs e)
    {
        if (sender is not MenuItem menuItem)
        {
            return;
        }

        var tag = menuItem.Tag?.ToString() ?? string.Empty;

        Debug.WriteLine($"Трей: {menuItem.Header}, тег: {tag}");

        if (_appPages.TryGetValue(tag, out Type? viewType))
        {
            _navigationService.Navigate(viewType);

            ShowAndActivateWindow();

            Debug.WriteLine($"Открываю страницу: {viewType}");

            return;
        }

        //если тега нет в appPages
        switch (tag)
        {
            case "tray_close":
                HandleTrayCloseClick();
                break;
            default:
                if (!string.IsNullOrEmpty(tag))
                {
                    Debug.WriteLine($"Неизвестный тег: {tag}");
                }
                break;
        }
    }
    private void SetupTrayMenuEvents()
    {
        foreach (var menuItem in TrayMenuItems)
        {
            if (menuItem is MenuItem item)
            {
                item.Click += OnTrayMenuItemClick;
            }
        }
    }
    private static void HandleTrayCloseClick()
    {
        Debug.WriteLine("Трей - закрытие приложения");

        Application.Current.Shutdown();
    }
    private void ShowAndActivateWindow()
    {
        if (Application.Current.MainWindow.WindowState == WindowState.Minimized)
        {
            Application.Current.MainWindow.WindowState = WindowState.Normal;
        }

        Application.Current.MainWindow.Show();
        Application.Current.MainWindow.Activate();
        Application.Current.MainWindow.Focus();
    }
}
