using admin.Core.Interfaces;
using admin.Features.Auth.Pages;
using admin.Features.Home;
using admin.Infrastructure.Services;
using admin.Shell.ViewModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using Wpf.Ui;
using Wpf.Ui.Abstractions;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace admin.Shell
{
    public partial class MainWindowView : FluentWindow, INavigationWindow
    {
        public MainWindowViewModel ViewModel { get; }

        private readonly IAppSettingsService _appSettingsService;
        public MainWindowView(
            MainWindowViewModel viewModel,
            INavigationService navigationService,
            IContentDialogService contentDialogService,
            IAppSettingsService appSettingsService
        )
        {
            ViewModel = viewModel;
            DataContext = viewModel;
            _appSettingsService = appSettingsService;

            SystemThemeWatcher.Watch(this);

            InitializeComponent();

            navigationService.SetNavigationControl(RootNavigation);
            contentDialogService.SetDialogHost(RootDialogHost);
        }
        public INavigationView GetNavigation() => RootNavigation;

        public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

        public void SetPageService(INavigationViewPageProvider navigationViewPageProvider) =>
            RootNavigation.SetPageProviderService(navigationViewPageProvider);

        public void ShowWindow() {
            Show();

            ViewModel.InitWindows();
        }

        public void CloseWindow() => Close();

        protected override void OnClosing(CancelEventArgs e)
        {
            _appSettingsService.SaveSettingsAsync();

            base.OnClosing(e);
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

        public void SetServiceProvider(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }
    }
}