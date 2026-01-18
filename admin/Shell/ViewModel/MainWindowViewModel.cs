using admin.Core.Interfaces;
using admin.Features.Auth;
using admin.Shell.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Controls;
using Wpf.Ui;
using Wpf.Ui.Controls;

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

    [ObservableProperty]
    private object _currentViewModel;

    public INavigationView RootNavigation { get; set; }

    private INavigationService _navigationService;
    private IAuthService _authService;
    private MainContentViewModel _mainContentViewModel;
    private bool _isLoaded = false;

    public MainWindowViewModel(
        AuthViewModel authViewModel,
        ShellLoadingViewModel shellLoadingViewModel,
        MainContentViewModel mainContentViewModel,
        INavigationService navigationService,
        IAuthService authService
    )
    {
        _mainContentViewModel = mainContentViewModel;
        _currentViewModel = shellLoadingViewModel;
        _navigationService = navigationService;
        _authService = authService;

        if (_isLoaded)
            return;
        
        if (!_authService.IsAuthenticated)
        {
            CurrentViewModel = authViewModel;
            ApplicationTitle = "Авторизация";
            return;
        }

        InitViewModel();
    }

    public void OnLoginSuccess()
    {
        CurrentViewModel = _mainContentViewModel;
        ApplicationTitle = "SpendScope";

        Debug.WriteLine("Логин удался!!!111"); //dgdsgdffdjnfsdnjkkjjsgkdkjkjsdkgksksdkbdsfkjfkjfdnskfjksdkjfkdsbfsk,sdnfsjfkjsdkdsnkjf
    }

    public void NavigateToMainWindow() => InitViewModel();
}
