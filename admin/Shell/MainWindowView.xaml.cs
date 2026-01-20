using admin.Features.Auth.Pages;
using admin.Features.Home;
using admin.Shell.ViewModel;
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

        public MainWindowView(
            MainWindowViewModel viewModel,
            INavigationService navigationService
        )
        {
            ViewModel = viewModel;
            DataContext = viewModel;

            SystemThemeWatcher.Watch(this);

            InitializeComponent();

            navigationService.SetNavigationControl(RootNavigation);
        }
        public INavigationView GetNavigation() => RootNavigation;

        public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

        public void SetPageService(INavigationViewPageProvider navigationViewPageProvider) =>
            RootNavigation.SetPageProviderService(navigationViewPageProvider);

        public void ShowWindow() {
            Show();

            ViewModel.InitNavigation();
        }

        public void CloseWindow() => Close();

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