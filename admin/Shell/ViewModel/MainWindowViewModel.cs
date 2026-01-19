using admin.Core.Interfaces;
using admin.Features.Auth.Pages;
using admin.Features.Home;
using admin.Infrastructure.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Wpf.Ui;

namespace admin.Shell.ViewModel;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private string _applicationTitle = "SpendScope";

    [ObservableProperty]
    private ObservableCollection<object> _navigationItems = [];

    [ObservableProperty]
    private ObservableCollection<object> _navigationFooter = [];

    [ObservableProperty]
    private ObservableCollection<Wpf.Ui.Controls.MenuItem> _trayMenuItems = [];

    private INavigationService _navigationService;
    private IAuthService _authService;
    private bool _isLoaded = false;

    private string CurrentWindow = string.Empty;

    public MainWindowViewModel()
    {
        _navigationService = App.GetRequiredService<INavigationService>();
        _authService = App.GetRequiredService<IAuthService>();

        if (_isLoaded)
            return;
        
        if (!_authService.IsAuthenticated)
        {
            InitAuthViewModel();
            return;
        }

        InitMainViewModel();
    }

    public void NavigateToAuthWindow()
    {
        InitAuthViewModel();
        _navigationService.Navigate(typeof(AuthLoginPage));
    }
    public void NavigateToMainWindow()
    {
        InitMainViewModel();
        _navigationService.Navigate(typeof(HomePage));
    }
}
