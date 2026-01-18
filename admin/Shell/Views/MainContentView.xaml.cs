using admin.Features.Home;
using admin.Shell.ViewModel;
using System.Windows.Controls;
using Wpf.Ui;

namespace admin.Shell.Views;

public partial class MainContentView : UserControl
{
    public MainContentView(MainWindowViewModel viewModel, INavigationService navigationService)
    {
        InitializeComponent();

        viewModel.RootNavigation = RootNavigation;

        navigationService.SetNavigationControl(RootNavigation);
        navigationService.Navigate(typeof(HomeView));
    }
}

