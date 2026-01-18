using admin.Core.Abstractions;
using admin.Features.Home;
using admin.Features.Settings;
using admin.Shell.ViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Navigation;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace admin.Shell.Views;

public partial class MainContentViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;

    public MainContentViewModel(MainWindowViewModel viewModel, INavigationService navigationService)
    {
        //_navigationService = navigationService;

        //viewModel.RootNavigation = RootNavigation;
        //_navigationService.SetNavigationControl(RootNavigation);
        //_navigationService.Navigate(typeof(HomeView));

    }
}
