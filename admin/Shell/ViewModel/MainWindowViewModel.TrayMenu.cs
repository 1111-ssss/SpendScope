using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace admin.Shell.ViewModel;
public partial class MainWindowViewModel
{
    public Dictionary<string, Action> TrayActions = [];

    private void OnTrayMenuItemClick(object sender, RoutedEventArgs e)
    {
        if (sender is not MenuItem menuItem)
        {
            return;
        }

        var tag = menuItem.Tag?.ToString() ?? string.Empty;

        Debug.WriteLine($"Трей: {menuItem.Header}, тег: {tag}");

        if (TrayActions.TryGetValue(tag, out Action? action))
        {
            action();

            //ShowAndActivateWindow();
            return;
        }
    }
    public void SetupTrayMenuEvents()
    {
        foreach (var menuItem in TrayMenuItems)
        {
            if (menuItem is MenuItem item)
            {
                item.Click -= OnTrayMenuItemClick;
                item.Click += OnTrayMenuItemClick;
            }
        }
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
