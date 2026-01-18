using admin.Shell.ViewModel;
using admin.Shell.Views;
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
        private INavigationView _rootNavigation { get; set; }

        public MainWindowView(MainWindowViewModel viewModel, INavigationService navigationService)
        {
            ViewModel = viewModel;
            DataContext = this;

            SystemThemeWatcher.Watch(this);

            InitializeComponent();

            _rootNavigation = viewModel.RootNavigation;
        }
        public INavigationView GetNavigation() => _rootNavigation;

        public bool Navigate(Type pageType) => _rootNavigation.Navigate(pageType);

        public void SetPageService(INavigationViewPageProvider navigationViewPageProvider) =>
            _rootNavigation.SetPageProviderService(navigationViewPageProvider);

        public void ShowWindow() => Show();

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