using admin.Views.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace admin.ViewModels.MainWindow;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private string _applicationTitle = string.Empty;

    [ObservableProperty]
    private ObservableCollection<object> _navigationItems = [];

    [ObservableProperty]
    private ObservableCollection<object> _navigationFooter = [];

    [ObservableProperty]
    private ObservableCollection<MenuItem> _trayMenuItems = [];

    private INavigationService _navigationService;
    private bool _isLoaded = false;

    public MainWindowViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;

        if (!_isLoaded)
            InitViewModel();
    }
    private void InitViewModel()
    {
        ApplicationTitle = "Панель администратора";

        NavigationItems =
        [
            new NavigationViewItem()
            {
                Content = "Главная",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                TargetPageType = typeof(Views.Pages.HomeView),
            },
            //new NavigationViewItem()
            //{
            //    Content = "Пользователи",
            //    Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
            //    TargetPageType = typeof(Views.Pages.UsersView),
            //},
        ];

        NavigationFooter =
        [
            new NavigationViewItem()
            {
                Content = "Настройки",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(Views.Pages.SettingsView),
            },
        ];

        TrayMenuItems = [new() { Header = "Home", Tag = "tray_home" }];

        SetupTrayMenuEvents();

        _isLoaded = true;
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
    private void OnTrayMenuItemClick(object sender, RoutedEventArgs e)
    {
        if (sender is not MenuItem menuItem)
        {
            return;
        }

        var tag = menuItem.Tag?.ToString() ?? string.Empty;

        Debug.WriteLine($"Трей: {menuItem.Header}, тег: {tag}");

        switch (tag)
        {
            case "tray_home":
                HandleTrayHomeClick();
                break;
            case "tray_settings":
                HandleTraySettingsClick();
                break;
            case "tray_close":
                HandleTrayCloseClick();
                break;
            default:
                if (!string.IsNullOrEmpty(tag))
                {
                    Debug.WriteLine($"unknown Tag: {tag}");
                }
                break;
        }
    }
    private void HandleTrayHomeClick()
    {
        Debug.WriteLine("Трей - Главное меню");

        ShowAndActivateWindow();

        _navigationService.Navigate(typeof(HomeView));
    }

    private void HandleTraySettingsClick()
    {
        Debug.WriteLine("Трей - Настройки");

        ShowAndActivateWindow();

        _navigationService.Navigate(typeof(SettingsView));
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
